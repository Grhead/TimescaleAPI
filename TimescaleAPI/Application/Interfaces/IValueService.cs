using TimescaleAPI.Application.DTOs;

namespace TimescaleAPI.Application.Interfaces;

public interface IValueService
{
    public Task<FileValuesDto> GetLastValues(string fileName);
}