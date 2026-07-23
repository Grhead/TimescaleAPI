using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Services;

public sealed class ResultCalculator : IResultCalculator
{
    public Result Calculate(IReadOnlyList<TimescaleData> records)
    {
        var first = records[0];

        var minDate = first.Date!.Value;
        var maxDate = first.Date.Value;

        var minValue = first.Value!.Value;
        var maxValue = first.Value.Value;

        double executionTimeSum = 0;
        double valueSum = 0;

        foreach (var record in records)
        {
            var date = record.Date!.Value;
            var value = record.Value!.Value;

            if (date < minDate)
                minDate = date;

            if (date > maxDate)
                maxDate = date;

            if (value < minValue)
                minValue = value;

            if (value > maxValue)
                maxValue = value;

            executionTimeSum += record.ExecutionTime!.Value;
            valueSum += value;
        }

        return new Result(
            (int)(maxDate - minDate).TotalSeconds,
            minDate.ToUniversalTime(),
            executionTimeSum / records.Count,
            valueSum / records.Count,
            CalculateMedian(records),
            maxValue,
            minValue);
    }
    
    private static double CalculateMedian(IReadOnlyList<TimescaleData> records)
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