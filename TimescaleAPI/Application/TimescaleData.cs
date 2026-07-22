using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application;

public record TimescaleData
{
    public DateTime Date { get; set; }
    public int ExecutionTime { get; set; }
    public double Value { get; set; }
}

public static class RecordConverting
{
    public static Value ToValueModel(this TimescaleData dto, Origin origin)
    {
        var newValue = new Value
        {
            Id = Guid.NewGuid(),
            Date = dto.Date.ToUniversalTime(),
            ExecutionTime = dto.ExecutionTime,
            IndicatorValue = dto.Value,
            Origin = origin
        };
        return newValue;
    }
}