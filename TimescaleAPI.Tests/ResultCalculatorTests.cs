using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Services;

namespace TimescaleAPI.Tests;

public class ResultCalculatorTests
{
    private readonly ResultCalculator _calculator = new();


    [Fact]
    public void SingleRecord_ReturnsRecordValues()
    {
        var records = new[] { new TimescaleValueDto(new DateTime(2026, 6, 15, 10, 0, 0, DateTimeKind.Utc), 100, 42.5) };

        var result = _calculator.Calculate(records);

        Assert.Equal(0, result.DeltaDate);
        Assert.Equal(42.5, result.AvgValue);
        Assert.Equal(42.5, result.MedianValue);
        Assert.Equal(100, result.AvgExecutionTime);
        Assert.Equal(42.5, result.MaxValue);
        Assert.Equal(42.5, result.MinValue);
    }

    [Fact]
    public void EvenRecordCount_MedianValue()
    {
        var records = new[]
        {
            new TimescaleValueDto(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 10.0),
            new TimescaleValueDto(new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), 200, 30.0),
            new TimescaleValueDto(new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), 300, 20.0),
            new TimescaleValueDto(new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc), 400, 40.0),
        };

        var result = _calculator.Calculate(records);

        Assert.Equal(25.0, result.MedianValue);
    }

    [Fact]
    public void OddRecordCount_MedianValue()
    {
        var records = new[]
        {
            new TimescaleValueDto(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 10.0),
            new TimescaleValueDto(new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), 200, 30.0),
            new TimescaleValueDto(new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc), 300, 20.0),
        };

        var result = _calculator.Calculate(records);

        Assert.Equal(20.0, result.MedianValue);
    }

    [Fact]
    public void DuplicateValues_MedianValue()
    {
        var records = new[]
        {
            new TimescaleValueDto(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 50.0),
            new TimescaleValueDto(new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc), 100, 50.0),
        };

        var result = _calculator.Calculate(records);

        Assert.Equal(50.0, result.MedianValue);
    }
    
    
    [Fact]
    public void CalculateStatsCorrectly()
    {
        var records = new[]
        {
            new TimescaleValueDto(new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 5.0),
            new TimescaleValueDto(new DateTime(2026, 6, 15, 0, 0, 0, DateTimeKind.Utc), 200, 100.0),
            new TimescaleValueDto(new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc), 300, 50.0),
        };

        var result = _calculator.Calculate(records);

        Assert.Equal(31449600, result.DeltaDate);
        Assert.Equal(51,66, result.AvgValue);
        Assert.Equal(100.0, result.MaxValue);
        Assert.Equal(5.0, result.MinValue);
        Assert.Equal(200, result.AvgExecutionTime);
    }
}
