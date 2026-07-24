namespace TimescaleAPI.Application.DTOs;

public record FileValuesDto(
    string FileName,
    TimescaleValueDto[] Values);