using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IValueRepository
{
    public Task<Origin> GetOrAddOrigin(string fileName, CancellationToken cancellationToken);
    public Task AddOrUpdateValues(Origin origin, List<Value> records, CancellationToken cancellationToken);
    
    public List<Value> GetLastValues(string fileName);
}