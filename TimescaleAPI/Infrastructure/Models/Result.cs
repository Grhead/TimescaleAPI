namespace TimescaleAPI.Infrastructure.Models;

public class Result
{
    public Guid Id { get; set; }
    public int DateDelta { get; set; }
    public DateTime DateMin { get; set; }
    public double ExecutionTimeAverage { get; set; }
    public double ValueAverage { get; set; }
    public double ValueMedian { get; set; }
    public double ValueMax { get; set; }
    public double ValueMin { get; set; }
    
    public Origin Origin { get; set; }
}