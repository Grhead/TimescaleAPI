using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FluentValidation;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;
using ValidationException = TimescaleAPI.Application.Exceptions.ValidationException;

namespace TimescaleAPI.Application.Services;

public class CsvParser(IValidator<TimescaleValueDto> validator) : IFileParser
{
    private const int MaxRecords = 10_000;
    
    public async Task<IReadOnlyList<TimescaleValueDto>> ParseAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        using var csv = new CsvReader(reader, config);
        await csv.ReadAsync();
        csv.ReadHeader();

        var records = new List<TimescaleValueDto>();
        while (await csv.ReadAsync())
            try
            {
                var rec = csv.GetRecord<TimescaleValueDto>();
                var result = await validator.ValidateAsync(rec, cancellationToken);
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