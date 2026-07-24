using System.Net;

namespace TimescaleAPI.Application.Exceptions;

public abstract class MetricsException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
    : Exception(message)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}