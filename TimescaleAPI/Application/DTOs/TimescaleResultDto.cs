namespace TimescaleAPI.Application.DTOs;

public record TimescaleResultDto(
    string FileName,
    long DeltaDate,
    DateTime MinDate,
    double AvgExecutionTime,
    double AvgValue,
    double MedianValue,
    double MaxValue,
    double MinValue
);