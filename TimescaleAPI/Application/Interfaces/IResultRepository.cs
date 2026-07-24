using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IResultRepository
{
    public Task AddOrUpdateResultAsync(Origin origin, Result result, CancellationToken cancellationToken);

    public Task<List<Result>> GetResultsByFiltersAsync(TimescaleFilterDto filterDto,
        CancellationToken cancellationToken);
}