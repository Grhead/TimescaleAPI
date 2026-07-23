using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IResultRepository
{
    public Task AddOrUpdateResult(Origin origin, Result result);
    public bool GetResultsByFilters(Origin origin); // TODO pass filters
    public bool GetLastResults(Origin origin);
}