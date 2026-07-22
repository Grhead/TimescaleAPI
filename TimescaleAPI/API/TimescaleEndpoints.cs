using System.Diagnostics;
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
                UploadService.ProcessUpload(file);

                return Results.Ok();
            })
            .DisableAntiforgery();

        app.MapGet("/results", async () => { });

        app.MapGet("/results/latest", async () => { });
    }
}