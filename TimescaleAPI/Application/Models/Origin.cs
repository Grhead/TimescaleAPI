namespace TimescaleAPI.Infrastructure.Models;

public class Origin
{
    public Guid Id { get; init; }
    public string NameHash { get; init; }
    
    public ICollection<Value> Values { get; init; }
    public ICollection<Result> Results { get; init; }

    public static Origin CreateOrigin(string nameHash)
    {
        return new Origin
        {
            Id = Guid.NewGuid(),
            NameHash = nameHash
        };
    }
}