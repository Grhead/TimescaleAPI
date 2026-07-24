namespace TimescaleAPI.Application.Models;

public class Result
{
    public Result(long deltaDate, DateTime minDate, double avgExecutionTime, double avgValue,
        double medianValue, double maxValue, double minValue)
    {
        Id = Guid.NewGuid();
        DeltaDate = deltaDate;
        MinDate = minDate;
        AvgExecutionTime = avgExecutionTime;
        AvgValue = avgValue;
        MedianValue = medianValue;
        MaxValue = maxValue;
        MinValue = minValue;
    }

    public Guid Id { get; private set; }

    public long DeltaDate { get; private set; }
    public DateTime MinDate { get; private set; }
    public double AvgExecutionTime { get; private set; }
    public double AvgValue { get; private set; }
    public double MedianValue { get; private set; }
    public double MaxValue { get; private set; }
    public double MinValue { get; private set; }

    public Guid OriginId { get; private set; }
    public Origin Origin { get; private set; }

    public void UpdateFrom(Result prevResult)
    {
        DeltaDate = prevResult.DeltaDate;
        MinDate = prevResult.MinDate;
        AvgExecutionTime = prevResult.AvgExecutionTime;
        AvgValue = prevResult.AvgValue;
        MedianValue = prevResult.MedianValue;
        MaxValue = prevResult.MaxValue;
        MinValue = prevResult.MinValue;
    }

    public void SetOrigin(Origin origin)
    {
        Origin = origin;
    }
}