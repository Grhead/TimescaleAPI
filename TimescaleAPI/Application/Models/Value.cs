namespace TimescaleAPI.Application.Models;

public class Value
{
    public Value(DateTime date, int executionTime, double indicatorValue)
    {
        Id = Guid.NewGuid();
        Date = date;
        ExecutionTime = executionTime;
        IndicatorValue = indicatorValue;
        
    }
    public Guid Id { get; private set; }
    public DateTime Date { get; private set; }
    public int ExecutionTime {get; private set;}
    public double IndicatorValue {get; private set;}

    public Guid OriginId { get; private set; }
    public Origin Origin { get; private set; }
    
    public void SetOrigin(Origin origin)
    {
        Origin = origin;
    }
    
    public void UpdateFrom(Value prevValue)
    {
        ExecutionTime = prevValue.ExecutionTime;
        IndicatorValue = prevValue.IndicatorValue;
    }
}