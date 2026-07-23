using TimescaleAPI.Application;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Infrastructure.Repositories;

public class ResultRepository : IResultRepository
{
    public bool CalculateResults(Origin origin, IList<TimescaleData> records)
    {
        throw new NotImplementedException();
    }

    public bool GetResultsByFilters(Origin origin)
    {
        throw new NotImplementedException();
    }

    public bool GetLastResults(Origin origin)
    {
        throw new NotImplementedException();
    }
}