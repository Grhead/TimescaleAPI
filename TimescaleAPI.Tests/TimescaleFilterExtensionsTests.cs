using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Services.Filters;

namespace TimescaleAPI.Tests;

public class TimescaleFilterExtensionsTests
{
    private static Origin CreateOrigin(string fileName)
    {
        var origin = new Origin(fileName);
        return origin;
    }

    private static Result CreateResult(Origin origin, double avgValue = 50.0, double avgExecTime = 100.0,
        DateTime? timestamp = null)
    {
        var result = new Result(0, timestamp ?? DateTime.MinValue, avgExecTime, avgValue, 0, 0, 0);
        result.SetOrigin(origin);
        return result;
    }

    [Fact]
    public void Apply_NoFilters_ReturnsAll()
    {
        var origin = CreateOrigin("data.csv");
        var results = new List<Result> { CreateResult(origin) };
        var filter = new TimescaleFilterDto(
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
    }

    [Fact]
    public void Apply_FilterByFileName_ReturnOnlyMatching()
    {
        var originA = CreateOrigin("report_a.csv");
        var originB = CreateOrigin("report_b.csv");
        var results = new List<Result>
        {
            CreateResult(originA),
            CreateResult(originB)
        };
        var filter = new TimescaleFilterDto(
            "report_a.csv",
            null,
            null,
            null,
            null,
            null,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
        Assert.Equal("report_a.csv", filtered[0].Origin.FileName);
    }

    [Fact]
    public void Apply_MinAvgValue_FilterCorrectly()
    {
        var low = CreateOrigin("low.csv");
        var high = CreateOrigin("high.csv");
        var results = new List<Result>
        {
            CreateResult(low, avgValue: 30.0),
            CreateResult(high, avgValue: 70.0)
        };
        var filter = new TimescaleFilterDto(
            null,
            null,
            null,
            50.0,
            null,
            null,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
        Assert.Equal(70.0, filtered[0].AvgValue);
    }

    [Fact]
    public void Apply_MaxAvgExecTime_FilterCorrectly()
    {
        var fast = CreateOrigin("fast.csv");
        var slow = CreateOrigin("slow.csv");
        var results = new List<Result>
        {
            CreateResult(fast, avgExecTime: 50.0),
            CreateResult(slow, avgExecTime: 200.0)
        };
        var filter = new TimescaleFilterDto(
            null,
            null,
            null,
            null,
            null,
            null,
            100.0);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
        Assert.Equal(50.0, filtered[0].AvgExecutionTime);
    }

    [Fact]
    public void Apply_CombinedFilters()
    {
        var a1 = CreateOrigin("a.csv");
        var a2 = CreateOrigin("a.csv");
        var b = CreateOrigin("b.csv");
        var results = new List<Result>
        {
            CreateResult(a1, avgValue: 30.0, avgExecTime: 50.0),
            CreateResult(a2, avgValue: 70.0, avgExecTime: 200.0),
            CreateResult(b, avgValue: 80.0, avgExecTime: 100.0)
        };
        var filter = new TimescaleFilterDto(
            "a.csv",
            null,
            null,
            50.0,
            null,
            100.0,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
        Assert.Equal(70.0, filtered[0].AvgValue);
    }

    [Fact]
    public void Apply_MaxAvgValue_FilterCorrectly()
    {
        var origin = CreateOrigin("data.csv");
        var results = new List<Result>
        {
            CreateResult(origin, avgValue: 40.0),
            CreateResult(origin, avgValue: 80.0)
        };
        var filter = new TimescaleFilterDto(
            null,
            null,
            null,
            null,
            50.0,
            null,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
        Assert.Equal(40.0, filtered[0].AvgValue);
    }

    [Fact]
    public void Apply_MinAvgExecTime_FilterCorrectly()
    {
        var origin = CreateOrigin("data.csv");
        var results = new List<Result>
        {
            CreateResult(origin, avgExecTime: 50.0),
            CreateResult(origin, avgExecTime: 150.0)
        };
        var filter = new TimescaleFilterDto(
            null,
            null,
            null,
            null,
            null,
            100.0,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Single(filtered);
        Assert.Equal(150.0, filtered[0].AvgExecutionTime);
    }


    [Fact]
    public void Apply_NoMatches_ReturnsEmpty()
    {
        var origin = CreateOrigin("data.csv");
        var results = new List<Result>
        {
            CreateResult(origin, avgValue: 20.0, avgExecTime: 30.0)
        };
        var filter = new TimescaleFilterDto(
            "miss.csv",
            null,
            null,
            null,
            null,
            null,
            null);

        var filtered = filter.Apply(results.AsQueryable()).ToList();

        Assert.Empty(filtered);
    }
}