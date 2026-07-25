using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Services.Calculations;

public sealed class ResultCalculator : IResultCalculator
{
    public Result Calculate(IReadOnlyList<Value> records)
    {
        var stats = new CommonResultCalc();
        foreach (var record in records)
            stats.Add(record);

        return new Result(
            stats.DeltaDate,
            stats.MinDate.ToUniversalTime(),
            stats.AvgExecutionTime,
            stats.AvgValue,
            CalculateMedian(records),
            stats.MaxValue,
            stats.MinValue);
    }

    private static double CalculateMedian(IReadOnlyList<Value> records)
    {
        var values = records
            .Select(r => r.IndicatorValue)
            .Order()
            .ToArray();

        var middle = values.Length / 2;

        return values.Length % 2 == 0
            ? (values[middle - 1] + values[middle]) / 2
            : values[middle];
    }
}