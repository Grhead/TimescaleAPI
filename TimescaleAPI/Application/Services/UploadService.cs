using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FluentValidation;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Utilities;
using ValidationException = TimescaleAPI.Application.Exceptions.ValidationException;

namespace TimescaleAPI.Application.Services;

public class UploadService(
    IValueRepository valueRepository,
    IResultRepository resultRepository,
    IUnitOfWork unitOfWork,
    IValidator<TimescaleValueDto> validator,
    IResultCalculator resultCalculator,
    ILogger<UploadService> logger) : IUploadService
{
    private const int MaxRecords = 10_000;

    public async Task<string> ProcessUpload(Stream stream, string rowFileName, CancellationToken cancellationToken)
    {
        var tsData = ParseUpload(stream);
        var fileName = Path.GetFileName(rowFileName);
        await unitOfWork.BeginAsync(cancellationToken);
        try
        {
            var origin = await valueRepository.GetOrAddOriginAsync(fileName, cancellationToken);
            var values = tsData.Select(x => x.ToValueModel(origin)).ToList();

            await valueRepository.ReplaceValuesAsync(origin, values, cancellationToken);

            var tsDataResult = resultCalculator.Calculate(tsData);
            await resultRepository.AddOrUpdateResultAsync(origin, tsDataResult, cancellationToken);

            await unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }

        return $"Successfully processed {tsData.Count} rows from {fileName}";
    }

    private List<TimescaleValueDto> ParseUpload(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();

        var records = new List<TimescaleValueDto>();
        while (csv.Read())
            try
            {
                var rec = csv.GetRecord<TimescaleValueDto>();
                var result = validator.Validate(rec);
                if (!result.IsValid) throw new ValidationException(result.ToDictionary());

                records.Add(rec);
                if (records.Count >= MaxRecords)
                    throw new ValidationException("File", $"File has more than {MaxRecords} records.");
            }
            catch (TypeConverterException ex)
            {
                throw new ValidationException("File",
                    $"Column {ex.Context.Reader.HeaderRecord[ex.Context.Reader.CurrentIndex]}, " +
                    $"Row {ex.Context.Parser.Row}: Invalid value type '{ex.Text}'.");
            }

        return records.Count == 0 ? throw new ValidationException("File", "File has no records.") : records;
    }
}