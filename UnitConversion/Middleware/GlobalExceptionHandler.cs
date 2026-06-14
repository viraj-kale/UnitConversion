using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UnitConversion.Exceptions;

namespace UnitConversion.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, problemDetails) = MapException(httpContext, exception);

        if (statusCode >= StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception processing request.");
        else
            _logger.LogWarning(exception, "Request failed with status code {StatusCode}.", statusCode);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static (int StatusCode, ProblemDetails ProblemDetails) MapException(
        HttpContext httpContext,
        Exception exception)
    {
        return exception switch
        {
            ValidationApiException validationException => (
                validationException.StatusCode,
                CreateValidationProblemDetails(httpContext, validationException)),
            ApiException apiException => (
                apiException.StatusCode,
                CreateProblemDetails(httpContext, apiException)),
            ArgumentException argumentException => (
                StatusCodes.Status400BadRequest,
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status400BadRequest,
                    "BAD_REQUEST",
                    "Bad request",
                    argumentException.Message)),
            _ => (
                StatusCodes.Status500InternalServerError,
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status500InternalServerError,
                    "INTERNAL_SERVER_ERROR",
                    "Internal server error",
                    "An unexpected error occurred while processing the request."))
        };
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ValidationApiException exception)
    {
        var problemDetails = new ValidationProblemDetails(exception.Errors)
        {
            Status = exception.StatusCode,
            Title = exception.Title,
            Detail = exception.Message,
            Instance = httpContext.Request.Path,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        problemDetails.Extensions["errorCode"] = exception.ErrorCode;
        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        ApiException exception)
    {
        var problemDetails = CreateProblemDetails(
            httpContext,
            exception.StatusCode,
            exception.ErrorCode,
            exception.Title,
            exception.Message);

        if (exception.Extensions is null)
            return problemDetails;

        foreach (var extension in exception.Extensions)
            problemDetails.Extensions[extension.Key] = extension.Value;

        return problemDetails;
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int statusCode,
        string errorCode,
        string title,
        string detail)
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path,
            Type = statusCode switch
            {
                StatusCodes.Status400BadRequest =>
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                StatusCodes.Status500InternalServerError =>
                    "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                _ => "about:blank"
            },
            Extensions =
            {
                ["errorCode"] = errorCode
            }
        };
    }
}
