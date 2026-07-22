using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using TimescaleAPI.Infrastructure;
using TimescaleAPI.Infrastructure.Models;

namespace TimescaleAPI.Application.Services;

public class UploadService(MetricsContext context, ILogger<UploadService> logger)
{
    public async Task<bool> ProcessUpload(IFormFile file)
    {
        var tsData = ParseUpload(file);
        var validationResult = ValidateRecords(tsData);
        if (!validationResult)
        {
            return false;
        }

        var fileName = GetFileNameHash(file);
        await SaveRecords(fileName, tsData);
        return true; // TODO change
    }

    private IList<TimescaleData> ParseUpload(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<TimescaleData>().ToList();
        return records;
    }

    private string GetFileNameHash(IFormFile file)
    {
        var fileName = Path.GetFileName(file.FileName);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(fileName)));
    }

    private bool ValidateRecords(IList<TimescaleData> records)
    {
        return true; // TODO release
    }

    private async Task SaveRecords(string fileName, IList<TimescaleData> records) // TODO to interface
    {
        var origin = await context.Origins.FirstOrDefaultAsync(x => x.NameHash == fileName);

        if (origin == null)
        {
            origin = Origin.CreateOrigin(fileName);
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