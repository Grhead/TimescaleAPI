using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IValueRepository
{
    public Task<Origin> GetOrAddOriginAsync(string fileName, CancellationToken cancellationToken);
    public Task AddOrUpdateValuesAsync(Origin origin, List<Value> records, CancellationToken cancellationToken);
    
    public Task<List<Value>> GetLastValues(string fileName);
}