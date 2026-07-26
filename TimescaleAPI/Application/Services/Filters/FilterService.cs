using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Mapping;

namespace TimescaleAPI.Application.Services.Filters;

public class FilterService(IResultRepository resultRepository) : IFilterService
{
    public async Task<List<TimescaleResultDto>> GetResults(TimescaleFilterDto filterDto,
        CancellationToken cancellationToken)
    {
        var resultsByFilters = await resultRepository.GetResultsByFiltersAsync(filterDto, cancellationToken);

        var resultDtoList = resultsByFilters.Select(x => x.ToResultDto(x.Origin.FileName)).ToList();
        return resultDtoList;
    }
}