using TimescaleAPI.Application;
using TimescaleAPI.Application.Services;

namespace TimescaleAPI.API;

public static class TimescaleEndpoints
{
    public static void RegisterTimescaleEndpoints(this WebApplication app)
    {
        app.MapPost("/upload", async (IFormFile file, UploadService uploadService, ILogger<UploadService> logger) =>
            {
                var result = await uploadService.ProcessUpload(file);

                return Results.Ok(result);
            })
            .DisableAntiforgery();

        app.MapGet("/results", async () => { });

        app.MapGet("/results/latest", async () => { });
    }
}