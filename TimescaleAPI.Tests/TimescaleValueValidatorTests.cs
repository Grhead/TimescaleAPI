using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Utilities;

namespace TimescaleAPI.Tests;

public class TimescaleValueValidatorTests
{
    private readonly TimescaleValueValidator _validator = new();

    private static TimescaleValueDto ValidDto()
        => new(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), 100, 42.5);

    [Fact]
    public void ValidDto_PassValidation()
    {
        var result = _validator.Validate(ValidDto());
        Assert.True(result.IsValid);
    }

    [Fact]
    public void NullDate_Fails()
    {
        var dto = new TimescaleValueDto(null, 100, 42.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Date");
    }

    [Fact]
    public void DateBeforeMinYear_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Utc), 100, 42.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Date");
    }

    [Fact]
    public void DateInFuture_Fails()
    {
        var futureDate = DateTime.UtcNow.AddDays(1);
        var dto = new TimescaleValueDto(futureDate, 100, 42.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Date");
    }

    [Fact]
    public void NullExecutionTime_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), null, 42.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ExecutionTime");
    }

    [Fact]
    public void ZeroExecutionTime_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), 0, 42.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ExecutionTime");
    }

    [Fact]
    public void NegativeExecutionTime_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), -5, 42.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "ExecutionTime");
    }

    [Fact]
    public void NullValue_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), 100, null);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Value");
    }

    [Fact]
    public void ZeroValue_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), 100, 0.0);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Value");
    }

    [Fact]
    public void NegativeValue_Fails()
    {
        var dto = new TimescaleValueDto(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), 100, -1.5);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Value");
    }

    [Fact]
    public void AllFieldsNull_ReturnsAllErrors()
    {
        var dto = new TimescaleValueDto(null, null, null);
        var result = _validator.Validate(dto);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 3);
    }
}
