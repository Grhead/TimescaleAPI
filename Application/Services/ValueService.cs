using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Utilities;

namespace TimescaleAPI.Application.Services;

public class ValueService(IValueRepository valueRepository)
{
    public FileValuesDto GetLastValues(string fileName)
    {
        var lastValues = valueRepository.GetLastValues(fileName);
        var fileValuesDto = new FileValuesDto(fileName, lastValues.Select(x => x.ToValuesDto()).ToArray());
        return fileValuesDto;
    }
}