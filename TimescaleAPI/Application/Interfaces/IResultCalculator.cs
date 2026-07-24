using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Models;

namespace TimescaleAPI.Application.Interfaces;

public interface IResultCalculator
{
    Result Calculate(IReadOnlyList<TimescaleValueDto> records);
}