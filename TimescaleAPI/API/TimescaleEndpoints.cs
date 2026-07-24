using TimescaleAPI.Application.DTOs;
using TimescaleAPI.Application.Interfaces;

namespace TimescaleAPI.API;

public static class TimescaleEndpoints
{
    private const int MaxFileSizeB = 10485760;

    private static readonly string[] AllowedFileTypes = [".csv"];

    public static void RegisterTimescaleEndpoints(this WebApplication app)
    {
        app.MapPost("/metrics",
                async (IFormFile file, IUploadService uploadService, CancellationToken cancellationToken) =>
                {
                    switch (file.Length)
                    {
                        case 0:
                            return Results.BadRequest("File is empty.");
                        case > MaxFileSizeB:
                            return Results.BadRequest("File is too large (> 10Mb)");
                    }

                    if (!AllowedFileTypes.Contains(Path.GetExtension(file.FileName), StringComparer.OrdinalIgnoreCase))
                        return Results.BadRequest("Invalid file type");

                    var result = await uploadService.ProcessUpload(file.OpenReadStream(), file.FileName,
                        cancellationToken);
                    return Results.Ok(result);
                })
            .DisableAntiforgery();

        app.MapGet("/results", async (IFilterService filterService, [AsParameters] TimescaleFilterDto filterDto,
            CancellationToken cancellationToken) =>
        {
            var results = await filterService.GetResults(filterDto, cancellationToken);
            return results.Count == 0 ? Results.NoContent() : Results.Ok(results);
        });

        app.MapGet("/values/latest", async (IValueService valueService, string fileName) =>
        {
            var fileValuesDto = await valueService.GetLastValues(fileName);
            return fileValuesDto.Values.Length == 0 ? Results.NoContent() : Results.Ok(fileValuesDto);
        });
    }
}