using TimescaleAPI.Application;
using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Services;

namespace TimescaleAPI.API;

public static class TimescaleEndpoints
{
    private const int MaxFileSizeB = 10485760;
    public static void RegisterTimescaleEndpoints(this WebApplication app)
    {
        app.MapPost("/metrics", async (IFormFile file, UploadService uploadService, CancellationToken cancellationToken) =>
            {
                switch (file.Length)
                {
                    case 0:
                        return Results.BadRequest("File is empty.");
                    case > MaxFileSizeB:
                        return Results.BadRequest("File is too large (> 10Mb)");
                    default:
                    {
                        var result = await uploadService.ProcessUpload(file.OpenReadStream(), file.FileName, cancellationToken);
                        return Results.Ok(result); // TODO return details
                    }
                }
            })
            .DisableAntiforgery();

        app.MapGet("/results", async (FilterService filterService, [AsParameters] TimescaleFilterDto filterDto, CancellationToken cancellationToken) =>
        {
            var results = await filterService.GetResults(filterDto, cancellationToken);
            return results.Count == 0 ? Results.NoContent() : Results.Ok(results);
        });

        app.MapGet("/results/latest", async () => { });
    }
}