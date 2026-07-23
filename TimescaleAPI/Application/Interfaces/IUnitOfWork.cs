namespace TimescaleAPI.Application.Interfaces;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}