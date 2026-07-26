namespace TimescaleAPI.Application.DTOs;

public record TimescaleFilterDto(
    string? FileName,
    DateTime? FromDate,
    DateTime? ToDate,
    double? MinAvgValue,
    double? MaxAvgValue,
    double? MinAvgExecTime,
    double? MaxAvgExecTime
);