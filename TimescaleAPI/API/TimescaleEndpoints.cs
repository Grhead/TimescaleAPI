using TimescaleAPI.Application.Services;

namespace TimescaleAPI.API;

public static class TimescaleEndpoints
{
    public static void RegisterTimescaleEndpoints(this WebApplication app)
    {
        app.MapPost("/upload", async (IFormFile file, UploadService uploadService, ILogger<UploadService> logger) =>
            {
                var result = await uploadService.ProcessUpload(file.OpenReadStream(), file.FileName);

                return Results.Ok(result); // TODO return details
            })
            .DisableAntiforgery();

        app.MapGet("/results", async () => { });

        app.MapGet("/results/latest", async () => { });
    }
}