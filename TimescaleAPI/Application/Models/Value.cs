using TimescaleAPI.Application;

namespace TimescaleAPI.Infrastructure.Models;

public class Value
{
    public Guid Id { get; init; }
    public DateTime Date { get; init; }
    public int ExecutionTime {get; set;}
    public double IndicatorValue {get; set;}

    public Guid OriginId { get; set; }
    public Origin Origin { get; init; }
}