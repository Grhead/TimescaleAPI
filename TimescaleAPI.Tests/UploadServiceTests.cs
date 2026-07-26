using System.Text;
using Microsoft.Extensions.Logging;
using NSubstitute;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Services;

namespace TimescaleAPI.Tests;

public class UploadServiceTests
{
    private readonly IValueRepository _valueRepo = Substitute.For<IValueRepository>();
    private readonly IResultRepository _resultRepo = Substitute.For<IResultRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IFileParser _fileParser = Substitute.For<IFileParser>();
    private readonly IResultCalculator _calculator = Substitute.For<IResultCalculator>();
    private readonly UploadService _service;

    public UploadServiceTests()
    {
        _service = new UploadService(
            _valueRepo, _resultRepo, _unitOfWork, _fileParser,
            _calculator);
    }

    [Fact]
    public async Task ProcessUpload_ValidCsv_ReturnsSuccessMessage()
    {
        _fileParser.ParseAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(new List<TimescaleValueDto>
            {
                new(new DateTime(2026, 6, 15, 10, 0, 0), 100, 42.5)
            });
        _valueRepo.GetOrAddOriginAsync("test.csv", Arg.Any<CancellationToken>())
            .Returns(new Origin("test.csv"));
        _calculator.Calculate(Arg.Any<List<Value>>())
            .Returns(new Result(0, DateTime.MinValue, 0, 0, 0, 0, 0));

        var csv = new MemoryStream(Encoding.UTF8.GetBytes(""));
        var result = await _service.ProcessUpload(csv, "test.csv", CancellationToken.None);

        Assert.Contains("1 row", result);
    }

    [Fact]
    public async Task ProcessUpload_CommitAsyncIsCalled()
    {
        _fileParser.ParseAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(new List<TimescaleValueDto>
            {
                new(new DateTime(2026, 6, 15, 10, 0, 0), 100, 42.5)
            });
        _valueRepo.GetOrAddOriginAsync("test.csv", Arg.Any<CancellationToken>())
            .Returns(new Origin("test.csv"));
        _calculator.Calculate(Arg.Any<List<Value>>())
            .Returns(new Result(0, DateTime.MinValue, 0, 0, 0, 0, 0));

        var csv = new MemoryStream(Encoding.UTF8.GetBytes(""));
        await _service.ProcessUpload(csv, "test.csv", CancellationToken.None);

        await _unitOfWork.Received(1).BeginAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessUpload_DatabaseError_RollbackAsyncCalled()
    {
        _fileParser.ParseAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(new List<TimescaleValueDto>
            {
                new(new DateTime(2026, 6, 15, 10, 0, 0), 100, 42.5)
            });
        _valueRepo.GetOrAddOriginAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<Origin>(new InvalidOperationException("DB error")));

        var csv = new MemoryStream(Encoding.UTF8.GetBytes(""));
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ProcessUpload(csv, "test.csv", CancellationToken.None));

        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ProcessUpload_ParserException()
    {
        _fileParser.ParseAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException<IReadOnlyList<TimescaleValueDto>>(new FormatException("bad format")));

        var csv = new MemoryStream(Encoding.UTF8.GetBytes(""));
        await Assert.ThrowsAsync<FormatException>(
            () => _service.ProcessUpload(csv, "test.csv", CancellationToken.None));
    }

    [Fact]
    public async Task ProcessUpload_RepositoryException()
    {
        _fileParser.ParseAsync(Arg.Any<Stream>(), Arg.Any<CancellationToken>())
            .Returns(new List<TimescaleValueDto>
            {
                new(new DateTime(2026, 6, 15, 10, 0, 0), 100, 42.5)
            });
        
        _valueRepo.GetOrAddOriginAsync("test.csv", Arg.Any<CancellationToken>())
            .Returns(new Origin("test.csv"));
        _calculator.Calculate(Arg.Any<List<Value>>())
            .Returns(new Result(0, DateTime.MinValue, 0, 0, 0, 0, 0));
        
        _valueRepo.ReplaceValuesAsync(Arg.Any<Origin>(), Arg.Any<List<Value>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromException(new InvalidOperationException("repo error")));

        var csv = new MemoryStream(Encoding.UTF8.GetBytes(""));
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.ProcessUpload(csv, "test.csv", CancellationToken.None));

        await _unitOfWork.Received(1).RollbackAsync(Arg.Any<CancellationToken>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
}
