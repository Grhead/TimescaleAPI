using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Utilities;

namespace TimescaleAPI.Application.Services;

public sealed class ResultCalculator : IResultCalculator
{
    public Result Calculate(IReadOnlyList<TimescaleValueDto> records)
    {
        var stats = new TimescaleStatistics();
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

    private static double CalculateMedian(IReadOnlyList<TimescaleValueDto> records)
    {
        var values = records
            .Select(r => r.Value!.Value)
            .Order()
            .ToArray();

        var middle = values.Length / 2;

        return values.Length % 2 == 0
            ? (values[middle - 1] + values[middle]) / 2
            : values[middle];
    }
}