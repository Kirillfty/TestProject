using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Test.Backend.Common.Exceptions;

public sealed class GlobalExceptionHandler : IExceptionHandler
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
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };

        switch (exception)
        {
            case NotFoundException ex:
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "Resource not found";
                problemDetails.Detail = ex.Message;
                break;

            case ValidationException ex:
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Validation error";
                problemDetails.Detail = ex.Message;
                break;

            default:
                _logger.LogError(exception, "Unhandled exception");

                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Internal server error";
                problemDetails.Detail = "An unexpected error occurred.";
                break;
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}