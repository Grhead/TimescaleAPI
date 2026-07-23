using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimescaleAPI.Application.Exceptions;

namespace TimescaleAPI.Application;

public class ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validation)
        {
            return false;
        }

        logger.LogWarning("Validation failed: {Message}", validation.Message);

        httpContext.Response.StatusCode = (int)validation.StatusCode;
        await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails
        {
            Status = 400,
            Title = "Validation Failed",
            Errors = validation.Errors
        }, cancellationToken);

        return true;
    }
}