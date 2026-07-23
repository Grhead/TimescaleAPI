namespace TimescaleAPI.Application.Models;

public class Value
{
    public Value()
    {
        
    }

    public Value(DateTime date, int executionTime, double indicatorValue, Origin origin)
    {
        Id = Guid.NewGuid();
        Date = date;
        ExecutionTime = executionTime;
        IndicatorValue = indicatorValue;
        Origin = origin;
    }
    public Guid Id { get; private set; }
    public DateTime Date { get; private set; }
    public int ExecutionTime {get; private set;}
    public double IndicatorValue {get; private set;}

    public Guid OriginId { get; private set; }
    public Origin Origin { get; private set; }
    
    public void UpdateFrom(Value prevValue)
    {
        ExecutionTime = prevValue.ExecutionTime;
        IndicatorValue = prevValue.IndicatorValue;
    }
}