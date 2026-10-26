using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VehicleManagement.Application.Exceptions;

namespace VehicleManagement.WebApi.ExceptionHandlers;

public class UnauthorizedExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not UnauthorizedException unauthorizedException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Não autorizado",
            Detail = unauthorizedException.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
