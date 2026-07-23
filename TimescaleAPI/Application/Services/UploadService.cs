using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FluentValidation;
using TimescaleAPI.Application.Interfaces;
using TimescaleAPI.Application.Utilities;
using ValidationException = TimescaleAPI.Application.Exceptions.ValidationException;

namespace TimescaleAPI.Application.Services;

public class UploadService(
    IValueRepository valueRepository, 
    IResultRepository resultRepository, 
    IUnitOfWork unitOfWork, 
    IValidator<TimescaleData> validator,
    IResultCalculator resultCalculator,
    ILogger<UploadService> logger)
{
    private const int MaxRecords = 10_000;

    public async Task<bool> ProcessUpload(Stream stream, string rowFileName)
    {
        var tsData = ParseUpload(stream);
        var fileName = GetFileNameHash(rowFileName);
        
        var origin = await valueRepository.GetOrAddOrigin(fileName);
        var values = tsData.Select(x => x.ToValueModel(origin)).ToList();
        
        await valueRepository.AddOrUpdateValues(origin, values);
        
        var tsDataResult = resultCalculator.Calculate(tsData);
        await resultRepository.AddOrUpdateResult(origin, tsDataResult);
        
        await unitOfWork.SaveChangesAsync();
        
        return true; // TODO change to detailed JSON
    }
    
    private string GetFileNameHash(string rowFileName)
    {
        var fileName = Path.GetFileName(rowFileName);
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(fileName)));
    }

    private List<TimescaleData> ParseUpload(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        using var csv = new CsvReader(reader, config);
        csv.Read();
        csv.ReadHeader();

        var records = new List<TimescaleData>();
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
}