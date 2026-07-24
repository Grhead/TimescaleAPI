using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Infrastructure.Repositories;

public class ValueRepository(MetricsContext context) : IValueRepository
{
    public async Task<Origin> GetOrAddOriginAsync(string fileName, CancellationToken cancellationToken)
    {
        var origin = await context.Origins.FirstOrDefaultAsync(x => x.FileName == fileName, cancellationToken);
        if (origin != null) return origin;

        origin = new Origin(fileName);
        try
        {
            await context.AddAsync(origin, cancellationToken);
        }
        catch (DbUpdateException)
        {
            context.Entry(origin).State = EntityState.Detached;
            origin = await context.Origins.FirstAsync(x => x.FileName == fileName, cancellationToken);
        }

        return origin;
    }

    public async Task AddOrUpdateValuesAsync(Origin origin, List<Value> values, CancellationToken cancellationToken)
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

    public async Task<List<Value>> GetLastValues(string fileName)
    {
         return await context.Values
            .AsNoTracking()
            .Where(x => x.Origin.FileName == fileName)
            .Include(result => result.Origin)
            .OrderBy(x => x.Date)
            .ToListAsync();
    }
}