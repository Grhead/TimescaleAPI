using TimescaleAPI.Application.Interfaces;

namespace TimescaleAPI.Infrastructure.Repositories;

public class UnitOfWork(MetricsContext context) : ICommitable
{
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}