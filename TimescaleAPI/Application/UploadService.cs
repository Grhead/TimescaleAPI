using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace TimescaleAPI.Application;

public class UploadService
{
    public static bool ProcessUpload(IFormFile file)
    {
        var tsData = ParseUpload(file);
        var validationResult = ValidateUpload(tsData);
        return validationResult; // TODO change
    }
    
    private static IEnumerable<TimescaleData> ParseUpload(IFormFile file)
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
        using var csv = new CsvReader(reader, config);
        var records = csv.GetRecords<TimescaleData>();
        return records;
    }

    private static bool ValidateUpload(IEnumerable<TimescaleData> records)
    {
        return false; // TODO release
    }

    
}