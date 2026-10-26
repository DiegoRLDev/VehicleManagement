using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VehicleManagement.Application.Exceptions;

namespace VehicleManagement.WebApi.ExceptionHandlers;

public class ConflictExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not ConflictException conflictException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflito",
            Detail = conflictException.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
