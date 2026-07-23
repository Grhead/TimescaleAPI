using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Utilities;

public static class TimescaleDataResults
{
    public static Result CalculateResults(this List<TimescaleData> records)
    {
        var finalResult = new Result
        (
            CalculateDateDelta(ref records),
            CalculateMinDate(ref records),
            CalculateAvgExecutionTime(ref records),
            CalculateAvgValue(ref records),
            CalculateMedianValue(ref records),
            CalculateMaxValue(ref records),
            CalculateMinValue(ref records)
        );
        return finalResult;
    }

    private static int CalculateDateDelta(ref List<TimescaleData> records)
    {
        return (records.Max(x => x.Date) - records.Min(x => x.Date)).Value.Seconds;
    }
    private static DateTime CalculateMinDate(ref List<TimescaleData> records)
    {
        return records.Min(x => x.Date).Value.ToUniversalTime();
    }
    private static double CalculateAvgExecutionTime(ref List<TimescaleData> records)
    {
        return records.Average(x => x.ExecutionTime).Value;
    }
    private static double CalculateAvgValue(ref List<TimescaleData> records)
    {
        return records.Average(x => x.Value).Value;
    }
    private static double CalculateMedianValue(ref List<TimescaleData> records)
    {
        var sorted = records.OrderBy(x => x.Value).ToArray();
        var median = sorted.Length % 2 == 0 
            ? (sorted[sorted.Length / 2 - 1].Value + sorted[sorted.Length / 2].Value) / 2.0 
            : sorted[sorted.Length / 2].Value;
        return median.Value;
    }
    private static double CalculateMaxValue(ref List<TimescaleData> records)
    {
        return records.Max(x => x.Value).Value;
    }
    private static double CalculateMinValue(ref List<TimescaleData> records)
    {
        return records.Min(x => x.Value).Value;
    }
}