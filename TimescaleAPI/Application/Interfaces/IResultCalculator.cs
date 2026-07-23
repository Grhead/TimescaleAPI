using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IResultCalculator
{
    Result Calculate(IReadOnlyList<TimescaleData> records);
}