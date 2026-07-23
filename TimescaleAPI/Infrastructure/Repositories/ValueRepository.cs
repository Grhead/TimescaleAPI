using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Infrastructure.Repositories;

public class ValueRepository(MetricsContext context) : IValueRepository
{
    public async Task<Origin> GetOrAddOrigin(string fileNameHash)
    {
        var origin = await context.Origins.FirstOrDefaultAsync(x => x.NameHash == fileNameHash);
        if (origin != null) return origin;

        origin = Origin.CreateOrigin(fileNameHash);
        await context.AddAsync(origin);

        return origin;
    }

    public async Task AddOrUpdateValues(Origin origin, List<Value> values)
    {
        var existingValues = await context.Values
            .Where(x => x.OriginId == origin.Id)
            .ToDictionaryAsync(x => x.Date);

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