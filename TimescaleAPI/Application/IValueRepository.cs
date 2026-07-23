using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application;

public interface IValueRepository
{
    public Origin GetOrCreateOrigin(string fileNameHash);
    public bool AddOrUpdateValues(Origin origin, IList<TimescaleData> records);
    public bool CalculateResults(Origin origin, IList<TimescaleData> records); // TODO mb change args
}