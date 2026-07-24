namespace TimescaleAPI.Application.Models;

public class Origin
{
    protected Origin()
    {
        
    }
    public Origin(string fileName)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
    }
    public Guid Id { get; private set; }
    public string FileName { get; private set; }
    
    public ICollection<Value> Values { get; set; } = new List<Value>();
    public ICollection<Result> Results { get; set; } = new List<Result>();
}