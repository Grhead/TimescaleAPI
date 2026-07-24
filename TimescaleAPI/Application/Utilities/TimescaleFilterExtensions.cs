using Microsoft.EntityFrameworkCore.Query;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Utilities;

public static class TimescaleFilterExtensions
{
    public static IQueryable<Result> Apply(
        this TimescaleFilterDto filterDto,
        IQueryable<Result> query)
    {
        if (!string.IsNullOrWhiteSpace(filterDto.FileName))
            query = query.Where(x => x.Origin.FileName == filterDto.FileName);

        if (filterDto.FromDate.HasValue)
            query = query.Where(x => x.Origin.Values.Any(v => v.Date >= filterDto.FromDate));
        if (filterDto.ToDate.HasValue)
            query = query.Where(x => x.Origin.Values.Any(v => v.Date <= filterDto.ToDate));

        if (filterDto.MinAvgValue.HasValue)
            query = query.Where(x => x.AvgValue >= filterDto.MinAvgValue);
        if (filterDto.MaxAvgValue.HasValue)
            query = query.Where(x => x.AvgValue <= filterDto.MaxAvgValue);

        if (filterDto.MinAvgExecTime.HasValue)
            query = query.Where(x => x.AvgExecutionTime >= filterDto.MinAvgExecTime);
        if (filterDto.MaxAvgExecTime.HasValue)
            query = query.Where(x => x.AvgExecutionTime <= filterDto.MaxAvgExecTime);

        return query;
    }
}