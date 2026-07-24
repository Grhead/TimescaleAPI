using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Utilities;

public static class TimescaleResultConverter
{
    public static TimescaleResultDto ToResultDto(this Result result, string fileName)
    {
        return new TimescaleResultDto(
            fileName, result.DeltaDate, result.MinDate, result.AvgExecutionTime,
            result.AvgValue, result.MedianValue, result.MaxValue, result.MinValue);
    }
}