using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Infrastructure.Repositories;

public class ResultRepository(MetricsContext context, ILogger<ResultRepository> logger) : IResultRepository
{
    public async Task AddOrUpdateResult(Origin origin, Result result)
    {
        var existingResult = await context.Results
            .FirstOrDefaultAsync(x => x.OriginId == origin.Id);

        if (existingResult is not null)
        {
            existingResult.UpdateFrom(result);
            return;
        }

        result.SetOrigin(origin);
        await context.Results.AddAsync(result);
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