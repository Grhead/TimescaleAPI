using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Tests;

public class ModelTests
{
    [Fact]
    public void Value_UpdateFrom_CopyExecutionTimeAndIndicatorValue()
    {
        var source = new Value(DateTime.UtcNow, 200, 55.5);
        var target = new Value(DateTime.UtcNow, 100, 10.0);

        target.UpdateFrom(source);

        Assert.Equal(200, target.ExecutionTime);
        Assert.Equal(55.5, target.IndicatorValue);
    }

    [Fact]
    public void Value_UpdateFrom_DoNotChangeDate()
    {
        var date = new DateTime(2025, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        var source = new Value(DateTime.UtcNow, 200, 55.5);
        var target = new Value(date, 100, 10.0);

        target.UpdateFrom(source);

        Assert.Equal(date, target.Date);
    }

    [Fact]
    public void Result_UpdateFrom_CopyAllFields()
    {
        var source = new Result(86400, new DateTime(2025, 1, 1), 150.0, 42.5, 40.0, 100.0, 5.0);
        var target = new Result(0, DateTime.MinValue, 0, 0, 0, 0, 0);

        target.UpdateFrom(source);

        Assert.Equal(86400, target.DeltaDate);
        Assert.Equal(new DateTime(2025, 1, 1), target.MinDate);
        Assert.Equal(150.0, target.AvgExecutionTime);
        Assert.Equal(42.5, target.AvgValue);
        Assert.Equal(40.0, target.MedianValue);
        Assert.Equal(100.0, target.MaxValue);
        Assert.Equal(5.0, target.MinValue);
    }
}
