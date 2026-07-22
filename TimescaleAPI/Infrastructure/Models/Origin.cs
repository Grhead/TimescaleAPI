namespace TimescaleAPI.Infrastructure.Models;

public class Origin
{
    public Guid Id { get; set; }
    public string NameHash { get; set; }
    
    public ICollection<Value> Values { get; set; }
    public ICollection<Result> Results { get; set; }
}