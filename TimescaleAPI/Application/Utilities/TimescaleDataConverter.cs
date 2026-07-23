using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Utilities;

public static class TimescaleDataConverter
{
    public static Value ToValueModel(this TimescaleData dto, Origin origin)
    {
        var newValue = new Value
        {
            Id = Guid.NewGuid(),
            Date = dto.Date!.Value.ToUniversalTime(),
            ExecutionTime = (int)dto.ExecutionTime!,
            IndicatorValue = (double)dto.Value!,
            Origin = origin
        };
        return newValue;
    }
}