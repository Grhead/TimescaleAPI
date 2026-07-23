namespace TimescaleAPI.Application;

public sealed class TimescaleStatistics
{
    private int _count;
    private double _executionTimeSum;
    private double _valueSum;

    public long DeltaDate => _count == 0 ? 0 : (long)(MaxDate - MinDate).TotalSeconds;

    public DateTime MinDate { get; private set; }
    private DateTime MaxDate { get; set; }

    public double AvgExecutionTime => _count == 0 ? 0 : _executionTimeSum / _count;

    public double AvgValue => _count == 0 ? 0 : _valueSum / _count;

    public double MaxValue { get; private set; }
    public double MinValue { get; private set; }

    public void Add(TimescaleData record)
    {
        var date = record.Date.Value;
        var value = record.Value.Value;
        var executionTime = record.ExecutionTime.Value;

        if (_count == 0)
        {
            MinDate = MaxDate = date;
            MinValue = MaxValue = value;
        }
        else
        {
            if (date < MinDate)
                MinDate = date;

            if (date > MaxDate)
                MaxDate = date;

            if (value < MinValue)
                MinValue = value;

            if (value > MaxValue)
                MaxValue = value;
        }

        _executionTimeSum += executionTime;
        _valueSum += value;
        _count++;
    }
}