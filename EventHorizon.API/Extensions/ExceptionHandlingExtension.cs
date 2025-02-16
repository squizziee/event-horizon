using EventHorizon.Contracts.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace EventHorizon.API.Extensions
{
    public static class ExceptionHandlingExtension
    {
        public static IServiceCollection AddGlobalExceptionHandling(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            return services;
        }
    }

    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var status = StatusCodes.Status500InternalServerError;
            string? message = null;

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
            else if (exception is ArgumentException)
            {
                status = StatusCodes.Status400BadRequest;
            }
            else if (exception is FluentValidation.ValidationException vex)
            {
                status = StatusCodes.Status400BadRequest;
            }

            var problemDetails = new ProblemDetails
            {
                Status = status,
                Title = message ?? exception.Message,
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;

        }
    }

}
