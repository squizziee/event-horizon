using EventHorizon.Contracts.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace EventHorizon.API.Extensions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var status = StatusCodes.Status418ImATeapot;

            if (exception is BadRequestException)
            {
                status = StatusCodes.Status400BadRequest;
            }
            else if (exception is InvalidCredentialException)
            {
                status = StatusCodes.Status401Unauthorized;
            }
            else if (exception is ResourceNotFoundException)
            {
                status = StatusCodes.Status404NotFound;
            }
            else if (exception is UnsupportedExtensionException)
            {
                status = StatusCodes.Status415UnsupportedMediaType;
            }
            else if (exception is ImmutableResourceException)
            {
                status = StatusCodes.Status403Forbidden;
            }
            else if (exception is PostgresException)
            {
                status = StatusCodes.Status409Conflict;
            }
            else
            {
                status = StatusCodes.Status400BadRequest;
            }

            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = exception.Message,
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;

        }
    }
}
