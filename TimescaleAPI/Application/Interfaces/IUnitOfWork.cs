namespace TimescaleAPI.Application.Interfaces;

public interface IUnitOfWork
{
    public Task BeginAsync(CancellationToken cancellationToken);
    public Task CommitAsync(CancellationToken cancellationToken);
    public Task RollbackAsync(CancellationToken cancellationToken);
}