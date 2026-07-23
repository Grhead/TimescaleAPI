namespace TimescaleAPI.Application.Models;

public class Origin
{
    public Guid Id { get; private set; }
    public string NameHash { get; private set; }

    public static Origin CreateOrigin(string nameHash)
    {
        return new Origin
        {
            Id = Guid.NewGuid(),
            NameHash = nameHash
        };
    }
}