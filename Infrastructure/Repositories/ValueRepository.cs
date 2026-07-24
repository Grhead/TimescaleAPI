using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Infrastructure.Repositories;

public class ValueRepository(MetricsContext context) : IValueRepository
{
    public async Task<Origin> GetOrAddOrigin(string fileName, CancellationToken cancellationToken)
    {
        var origin = await context.Origins.FirstOrDefaultAsync(x => x.FileName == fileName, cancellationToken);
        if (origin != null) return origin;

        origin = new Origin(fileName);
        await context.AddAsync(origin, cancellationToken);

        return origin;
    }

    public async Task AddOrUpdateValues(Origin origin, List<Value> values, CancellationToken cancellationToken)
    {
        var existingValues = await context.Values
            .Where(x => x.OriginId == origin.Id)
            .ToDictionaryAsync(x => x.Date, cancellationToken);

        foreach (var value in values)
        {
            if (existingValues.TryGetValue(value.Date, out var entity))
            {
                entity.UpdateFrom(value);
            }
            else
            {
                context.Values.Add(value);
            }
        }
    }
}