using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Utilities;

public static class TimescaleDataConverter
{
    public static Value ToValueModel(this TimescaleData dto, Origin origin)
    {
        var newValue = new Value
        (
            dto.Date.Value.ToUniversalTime(),
            (int)dto.ExecutionTime,
            (double)dto.Value,
            origin
        );
        return newValue;
    }
}