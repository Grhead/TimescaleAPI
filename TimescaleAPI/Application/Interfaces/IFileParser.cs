using TimescaleAPI.Application.DTOs;

namespace TimescaleAPI.Application.Interfaces;

public interface IFileParser
{
    Task<IReadOnlyList<TimescaleValueDto>> ParseAsync(Stream stream, CancellationToken cancellationToken);
}