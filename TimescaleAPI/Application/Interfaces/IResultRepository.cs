using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IResultRepository
{
    public bool CalculateResults(Origin origin, IList<TimescaleData> records);
    public bool GetResultsByFilters(Origin origin); // TODO pass filters
    public bool GetLastResults(Origin origin);
}