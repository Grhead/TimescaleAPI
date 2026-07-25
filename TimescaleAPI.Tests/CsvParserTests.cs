using System.Text;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Services;
using ValidationException = TimescaleAPI.Application.Exceptions.ValidationException;

namespace TimescaleAPI.Tests;

public class CsvParserTests
{
    private readonly IValidator<TimescaleValueDto> _validator = Substitute.For<IValidator<TimescaleValueDto>>();
    private readonly CsvParser _parser;

    public CsvParserTests()
    {
        _validator.ValidateAsync(Arg.Any<TimescaleValueDto>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _parser = new CsvParser(_validator);
    }

    [Fact]
    public async Task ParseAsync_EmptyFile_ThrowValidationException()
    {
        var csv = new MemoryStream("Date;ExecutionTime;Value\n"u8.ToArray());

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _parser.ParseAsync(csv, CancellationToken.None));

        Assert.Contains("no records", string.Join(" ", ex.Errors.Values.SelectMany(e => e)));
    }

    [Fact]
    public async Task ParseAsync_InvalidColumnType_ThrowValidationException()
    {
        var csv = new MemoryStream("Date;ExecutionTime;Value\n2026-06-15T10:00:00;not_a_number;42.5\n"u8.ToArray());

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _parser.ParseAsync(csv, CancellationToken.None));

        Assert.Contains("Invalid value type", string.Join(" ", ex.Errors.Values.SelectMany(e => e)));
    }

    [Fact]
    public async Task ParseAsync_MoreThan10000Records_ThrowValidationException()
    {
        var sb = new StringBuilder("Date;ExecutionTime;Value\n");
        for (var i = 0; i < 10_001; i++)
            sb.AppendLine($"2026-01-01T00:00:00;{i + 1};{(double)i + 1}");

        var csv = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));

        var ex = await Assert.ThrowsAsync<ValidationException>(
            () => _parser.ParseAsync(csv, CancellationToken.None));

        Assert.Contains("more than", string.Join(" ", ex.Errors.Values.SelectMany(e => e)));
    }

    [Fact]
    public async Task ParseAsync_ValidCsv()
    {
        var csv = new MemoryStream("Date;ExecutionTime;Value\n2026-06-15T10:00:00;100;42.5\n"u8.ToArray());

        var result = await _parser.ParseAsync(csv, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(new DateTime(2026, 6, 15, 10, 0, 0), result[0].Date);
        Assert.Equal(100, result[0].ExecutionTime);
        Assert.Equal(42.5, result[0].Value);
    }
}