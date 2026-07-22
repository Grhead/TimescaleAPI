using TimescaleAPI.Application;

namespace TimescaleAPI.Infrastructure.Models;

public class Value
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public int ExecutionTime {get; set;}
    public double IndicatorValue {get; set;}

    public Origin Origin { get; set; }
}