using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Utilities;

public static class TimescaleValueConverter
{
    public static Value ToValueModel(this TimescaleValueDto dto, Origin origin)
    {
        var newValue = new Value
        (
            dto.Date.Value.ToUniversalTime(),
            (int)dto.ExecutionTime,
            (double)dto.Value
        );
        newValue.SetOrigin(origin);
        return newValue;
    }

    public static TimescaleValueDto ToValuesDto(this Value value)
    {
        return new TimescaleValueDto(value.Date, value.ExecutionTime, value.IndicatorValue);
    }
}