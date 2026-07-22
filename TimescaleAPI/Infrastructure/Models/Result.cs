namespace TimescaleAPI.Infrastructure.Models;

public class Result
{
    public Guid Id { get; init; }
    public int DateDelta { get; init; }
    public DateTime DateMin { get; init; }
    public double ExecutionTimeAverage { get; init; }
    public double ValueAverage { get; init; }
    public double ValueMedian { get; init; }
    public double ValueMax { get; init; }
    public double ValueMin { get; init; }
    
    public Guid OriginId { get; set; }
    public Origin Origin { get; init; }
}