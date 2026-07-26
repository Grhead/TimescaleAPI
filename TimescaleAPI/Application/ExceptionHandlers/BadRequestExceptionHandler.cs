using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TimescaleAPI.Application.ExceptionHandlers;

public class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BadHttpRequestException badRequestException) return false;

        logger.LogWarning("BadRequest failed: {Message}", badRequestException.Message);

        httpContext.Response.StatusCode = badRequestException.StatusCode;

        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = httpContext.Response.StatusCode,
            Title = "Invalid request parameters",
            Detail = "One or more query parameters have an invalid format."
        }, cancellationToken);

        return true;
    }
}