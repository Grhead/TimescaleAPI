using TimescaleAPI.Application;
using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Infrastructure;

public class ValueRepository : IValueRepository
{
    public Origin GetOrCreateOrigin(string fileNameHash)
    {
        throw new NotImplementedException();
    }

    public bool AddOrUpdateValues(Origin origin, IList<TimescaleData> records)
    {
        throw new NotImplementedException();
    }

    public bool CalculateResults(Origin origin, IList<TimescaleData> records)
    {
        throw new NotImplementedException();
    }
}