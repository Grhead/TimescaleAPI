namespace TimescaleAPI.Application;

public record TimescaleData
{
    public DateTime? Date { get; set; }
    public int? ExecutionTime { get; set; }
    public double? Value { get; set; }
}

