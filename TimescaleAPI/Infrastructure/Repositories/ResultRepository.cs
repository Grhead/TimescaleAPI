using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;
using TimescaleAPI.Application.Utilities;

namespace TimescaleAPI.Infrastructure.Repositories;

public class ResultRepository(MetricsContext context, ILogger<ResultRepository> logger) : IResultRepository
{
    public async Task AddOrUpdateResult(Origin origin, Result result, CancellationToken cancellationToken)
    {
        var existingResult = await context.Results
            .FirstOrDefaultAsync(x => x.OriginId == origin.Id, cancellationToken);

        if (existingResult is not null)
        {
            existingResult.UpdateFrom(result);
            return;
        }

        result.SetOrigin(origin);
        await context.Results.AddAsync(result, cancellationToken);
    }

    public async Task<List<Result>> GetResultsByFiltersAsync(TimescaleFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var resultsQuery = context.Results.AsNoTracking();

        resultsQuery = filterDto.Apply(resultsQuery);

        return await resultsQuery.Include(x => x.Origin).ToListAsync(cancellationToken);
    }

    public async Task<string?> GetFileNameByOrigin(
        Guid originId,
        CancellationToken cancellationToken)
    {
        return await context.Origins
            .Where(x => x.Id == originId)
            .Select(x => x.FileName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public bool GetLastResults(Origin origin)
    {
        throw new NotImplementedException();
    }
}