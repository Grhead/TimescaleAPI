using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Services;
using ValidationException = TimescaleAPI.Application.Exceptions.ValidationException;

namespace TimescaleAPI.Tests;

public class UploadServiceTests
{
    private readonly IValueRepository _valueRepo = Substitute.For<IValueRepository>();
    private readonly IResultRepository _resultRepo = Substitute.For<IResultRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IValidator<TimescaleValueDto> _validator = Substitute.For<IValidator<TimescaleValueDto>>();
    private readonly IResultCalculator _calculator = Substitute.For<IResultCalculator>();
    private readonly UploadService _service;

    public UploadServiceTests()
    {
        _validator.Validate(Arg.Any<TimescaleValueDto>())
            .Returns(new ValidationResult());

        _service = new UploadService(
            _valueRepo, _resultRepo, _unitOfWork,
            _validator, _calculator,
            Substitute.For<ILogger<UploadService>>());
    }

    private static Stream CsvStream(string rows)
    {
        var content = "Date;ExecutionTime;Value\n" + rows;
        var bytes = Encoding.UTF8.GetBytes(content);
        return new MemoryStream(bytes);
    }

    [Fact]
    public async Task ValidCsv_ProcessSuccessfullyOneRow()
    {
        var csv = CsvStream("2026-06-15T10:00:00;100;42.5\n");
        var origin = new Origin("test.csv");

        _valueRepo.GetOrAddOriginAsync("test.csv", Arg.Any<CancellationToken>())
            .Returns(origin);
        _calculator.Calculate(Arg.Any<List<TimescaleValueDto>>())
            .Returns(new Result(0, DateTime.MinValue, 0, 0, 0, 0, 0));

        var result = await _service.ProcessUpload(csv, "test.csv", CancellationToken.None);

        Assert.Contains("1 row", result);
        await _unitOfWork.Received(1).BeginAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().RollbackAsync(Arg.Any<CancellationToken>());
    }
    
    [Fact]
    public async Task ValidCsv_ProcessSuccessfullyManyRows()
    {
        var csv = CsvStream("2026-06-15T10:00:00;100;42.5\n2026-07-16T10:00:00;10;12.3\n");
        var origin = new Origin("test.csv");

        _valueRepo.GetOrAddOriginAsync("test.csv", Arg.Any<CancellationToken>())
            .Returns(origin);
        _calculator.Calculate(Arg.Any<List<TimescaleValueDto>>())
            .Returns(new Result(0, DateTime.MinValue, 0, 0, 0, 0, 0));

        var result = await _service.ProcessUpload(csv, "test.csv", CancellationToken.None);

        Assert.Contains("2 rows", result);
        await _unitOfWork.Received(1).BeginAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().RollbackAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task EmptyCsv_ThrowValidationException()
    {
        var csv = CsvStream("");

        await Assert.ThrowsAsync<ValidationException>(
            () => _service.ProcessUpload(csv, "empty.csv", CancellationToken.None));
    }

    [Fact]
    public async Task MoreThan10000Records_ThrowValidationException()
    {
        var sb = new StringBuilder("Date;ExecutionTime;Value\n");
        for (var i = 0; i < 10_001; i++)
            sb.AppendLine($"2026-01-01T00:00:00;{i + 1};{(double)i + 1}");

        var csv = CsvStream(sb.ToString());

        await Assert.ThrowsAsync<ValidationException>(
            () => _service.ProcessUpload(csv, "big.csv", CancellationToken.None));
    }

    [Fact]
    public async Task InvalidColumnType_ThrowValidationException()
    {
        var csv = CsvStream("2026-06-15T10:00:00;not_a_number;42.5\n");

        await Assert.ThrowsAsync<ValidationException>(
            () => _service.ProcessUpload(csv, "bad.csv", CancellationToken.None));
    }

    [Fact]
    public async Task DatabaseError_RollBackTransaction()
    {
        var csv = CsvStream("2026-06-15T10:00:00;100;42.5\n");

        _valueRepo.GetOrAddOriginAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Origin>(new InvalidOperationException("DB error")));

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ProcessUpload(csv, "test.csv", CancellationToken.None));

        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
}
