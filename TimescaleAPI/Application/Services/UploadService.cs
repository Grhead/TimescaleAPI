using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Infrastructure;
using TimescaleAPI.Application.Utilities;
using TimescaleAPI.Infrastructure.Models;
using ValidationException = TimescaleAPI.Application.Exceptions.ValidationException;

namespace TimescaleAPI.Application.Services;

public class UploadService(MetricsContext context, IValidator<TimescaleData> validator, ILogger<UploadService> logger)
{
    private const int MaxRecords = 10_000;

    public async Task<bool> ProcessUpload(Stream stream, string rowFileName)
    {
        var tsData = ParseUpload(stream);

        var fileName = GetFileNameHash(rowFileName);
        await SaveRecords(fileName, tsData);
        return true; // TODO change
    }

    private List<TimescaleData> ParseUpload(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        var records = new List<TimescaleData>();
        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            try
            {
                var rec = csv.GetRecord<TimescaleData>();
                var result = validator.Validate(rec);
                if (!result.IsValid)
                {
                    throw new ValidationException(result.ToDictionary());
                }

                records.Add(rec);
                if (records.Count >= MaxRecords)
                {
                    throw new ValidationException("File", $"File has more than {MaxRecords} records.");
                }
            }
            catch (TypeConverterException ex)
            {
                    throw new ValidationException("File",
                        $"Column {ex.Context.Reader.HeaderRecord[ex.Context.Reader.CurrentIndex]}, Row {ex.Context.Parser.Row}: Invalid value type '{ex.Text}'.");
            }
        }

        return records.Count == 0 ? throw new ValidationException("File", "File has no records.") : records;
    }

    private string GetFileNameHash(string rowFileName)
    {
        var fileName = Path.GetFileName(rowFileName);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(fileName)));
    }

    private async Task SaveRecords(string fileNameHash, IList<TimescaleData> records) // TODO to interface
    {
        var origin = await context.Origins.FirstOrDefaultAsync(x => x.NameHash == fileNameHash);

        if (origin == null)
        {
            origin = Origin.CreateOrigin(fileNameHash);
            await context.AddAsync(origin);
        }

        var values = records.Select(x => x.ToValueModel(origin)).ToList();
        var existingValues = await context.Values
            .Where(x => x.OriginId == origin.Id)
            .ToDictionaryAsync(x => x.Date);

        foreach (var value in values)
        {
            if (existingValues.TryGetValue(value.Date, out var entity))
            {
                entity.ExecutionTime = value.ExecutionTime;
                entity.IndicatorValue = value.IndicatorValue;
            }
            else
            {
                context.Values.Add(value);
            }
        }

        await context.SaveChangesAsync(); // TODO remove 
    }
}