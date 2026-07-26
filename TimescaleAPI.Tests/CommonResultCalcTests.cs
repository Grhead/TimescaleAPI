using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Services.Calculations;

namespace TimescaleAPI.Tests;

public class CommonResultCalcTests
{
    [Fact]
    public void EmptyStats_ReturnsZeroDefaults()
    {
        var stats = new CommonResultCalc();

        Assert.Equal(0, stats.DeltaDate);
        Assert.Equal(0, stats.AvgExecutionTime);
        Assert.Equal(0, stats.AvgValue);
    }

    [Fact]
    public void SingleRecord_StatsMatchRecordValues()
    {
        var stats = new CommonResultCalc();
        var dto = new Value(
            new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc),
            100,
            42.5);

        stats.Add(dto);

        Assert.Equal(0, stats.DeltaDate);
        Assert.Equal(new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc), stats.MinDate);
        Assert.Equal(100, stats.AvgExecutionTime);
        Assert.Equal(42.5, stats.AvgValue);
        Assert.Equal(42.5, stats.MaxValue);
        Assert.Equal(42.5, stats.MinValue);
    }

    [Fact]
    public void MultipleRecords_CalculateCorrectAverages()
    {
        var stats = new CommonResultCalc();

        stats.Add(new Value(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 10.0));
        stats.Add(new Value(new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc), 200, 20.0));
        stats.Add(new Value(new DateTime(2025, 1, 3, 0, 0, 0, DateTimeKind.Utc), 300, 30.0));

        Assert.Equal(200, stats.AvgExecutionTime);
        Assert.Equal(20.0, stats.AvgValue); 
    }

    [Fact]
    public void MultipleRecords_CalculateDeltaDateInSeconds()
    {
        var stats = new CommonResultCalc();

        stats.Add(new Value(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 10.0));
        stats.Add(new Value(new DateTime(2025, 1, 1, 0, 0, 10, DateTimeKind.Utc), 200, 20.0));

        Assert.Equal(10, stats.DeltaDate);
    }

    [Fact]
    public void MultipleRecords_TrackMinMaxDates()
    {
        var stats = new CommonResultCalc();

        stats.Add(new Value(new DateTime(2025, 6, 15, 12, 0, 0, DateTimeKind.Utc), 100, 10.0));
        stats.Add(new Value(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 200, 20.0));
        stats.Add(new Value(new DateTime(2025, 12, 31, 23, 59, 59, DateTimeKind.Utc), 300, 30.0));

        Assert.Equal(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), stats.MinDate);
        Assert.Equal(30.0, stats.MaxValue);
        Assert.Equal(10.0, stats.MinValue);
    }

    [Fact]
    public void DuplicateValues_MaxMinAreEqual()
    {
        var stats = new CommonResultCalc();

        stats.Add(new Value(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc), 100, 50.0));
        stats.Add(new Value(new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc), 100, 50.0));

        Assert.Equal(50.0, stats.MaxValue);
        Assert.Equal(50.0, stats.MinValue);
    }
}
