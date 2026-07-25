using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Mapping;

namespace TimescaleAPI.Application.Services;

public class UploadService(
    IValueRepository valueRepository,
    IResultRepository resultRepository,
    IUnitOfWork unitOfWork,
    IFileParser fileParser,
    IResultCalculator resultCalculator,
    ILogger<UploadService> logger) : IUploadService
{
    public async Task<string> ProcessUpload(Stream stream, string rowFileName, CancellationToken cancellationToken)
    {
        try
        {
            var tsData = await fileParser.ParseAsync(stream, cancellationToken);
            var fileName = Path.GetFileName(rowFileName);
            await unitOfWork.BeginAsync(cancellationToken);

            var origin = await valueRepository.GetOrAddOriginAsync(fileName, cancellationToken);
            var values = tsData.Select(x => x.ToValueModel(origin)).ToList();

            await valueRepository.ReplaceValuesAsync(origin, values, cancellationToken);

            var tsDataResult = resultCalculator.Calculate(values);
            await resultRepository.AddOrUpdateResultAsync(origin, tsDataResult, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
            var grammar = tsData.Count == 1 ? "row" : "rows";

            return $"Successfully processed {tsData.Count} {grammar} from {fileName}";
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}