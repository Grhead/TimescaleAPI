using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IValueRepository
{
    public Task<Origin> GetOrCreateOrigin(string fileNameHash);
    public Task AddOrUpdateValues(Origin origin, List<Value> records);
}