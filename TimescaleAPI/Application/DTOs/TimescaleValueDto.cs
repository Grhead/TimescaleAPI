namespace TimescaleAPI.Application.DTOs;

public record TimescaleValueDto(
    DateTime? Date,
    int? ExecutionTime,
    double? Value);