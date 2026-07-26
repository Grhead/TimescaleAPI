using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Mapping;

namespace TimescaleAPI.Application.Services;

public class ValueService(IValueRepository valueRepository) : IValueService
{
    public async Task<FileValuesDto> GetLastValues(string fileName)
    {
        var lastValues = await valueRepository.GetLastValuesAsync(fileName);
        var fileValuesDto = new FileValuesDto(fileName, lastValues.Select(x => x.ToValuesDto()).ToArray());
        return fileValuesDto;
    }
}