namespace TimescaleAPI.Application.Models;

public class Origin
{
    public Origin(string fileName)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
    }

    public Guid Id { get; private set; }
    public string FileName { get; private set; }

    public ICollection<Value> Values { get; private set; }
    public ICollection<Result> Results { get; private set; }
}