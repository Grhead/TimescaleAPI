namespace TimescaleAPI.Application.Interfaces;

public interface ICommitable
{
    public Task SaveChangesAsync();
}