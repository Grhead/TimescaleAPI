using TimescaleAPI.Application.DTOs;

namespace TimescaleAPI.Application.Interfaces;

public interface IFilterService
{
    public Task<List<TimescaleResultDto>> GetResults(TimescaleFilterDto filterDto, CancellationToken cancellationToken);
}