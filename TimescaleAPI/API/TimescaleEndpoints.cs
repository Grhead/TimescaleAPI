using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using TimescaleAPI.Application;

namespace TimescaleAPI.API;

public static class TimescaleEndpoints
{
    public static void RegisterTimescaleEndpoints(this WebApplication app)
    {
        app.MapPost("/upload", async (IFormFile file) =>
        {
            using var reader = new StreamReader(file.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" };
            using var csv = new CsvReader(reader, config);
            
            var records = csv.GetRecords<TimescaleData>().ToList();
            return Results.Ok(records);
        })
            .DisableAntiforgery();

        app.MapGet("/results", async () => { });

        app.MapGet("/results/latest", async () => { });
    }
}