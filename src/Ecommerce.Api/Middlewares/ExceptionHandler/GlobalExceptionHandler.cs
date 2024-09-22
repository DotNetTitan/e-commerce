using System.Net;
using System.Text.Json;
using Ecommerce.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Middlewares.ExceptionHandler
{
    /// <summary>
    /// Represents a global exception handler middleware that processes exceptions during the request pipeline.
    /// </summary>
    /// <remarks>
    /// This middleware handles the following exceptions:
    /// <list type="bullet">
    /// <item>
    /// <description><see cref="CustomerNotFoundException"/>: Returns 404 NotFound response.</description>
    /// </item>
    /// <item>
    /// <description><see cref="OrderNotFoundException"/>: Returns 404 NotFound response.</description>
    /// </item>
    ///   /// <item>
    /// <description><see cref="ProductNotFoundException"/>: Returns 404 NotFound response.</description>
    /// </item>
    /// <item>
    /// <description><see cref="UserNotFoundException"/>: Returns 404 NotFound response.</description>
    /// </item>
    /// <item>
    /// <description>Any other exception: Returns a 500 Internal Server Error.</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class GlobalExceptionHandler(IWebHostEnvironment env, ILogger<GlobalExceptionHandler> logger)
        : IExceptionHandler
    {
        private readonly IWebHostEnvironment _env = env;
        private readonly ILogger<GlobalExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An exception occurred while processing the request.");

            var problemDetails = new ProblemDetails();
            problemDetails.Instance = httpContext.Request.Path;

            if (exception is ValidationException validationFailedException)
            {
                problemDetails.Title = "One or more validation errors occurred.";
                problemDetails.Type = RfcTypeUrls.BadRequest;
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Status = StatusCodes.Status400BadRequest;
                var validationErrors = validationFailedException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                problemDetails.Extensions.Add("errors", validationErrors);
            }
            else
            {
                var (statusCode, title) = exception switch
                {
                    CustomerNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                    OrderNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                    ProductNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                    UserNotFoundException => ((int)HttpStatusCode.NotFound, exception.Message),
                    _ => ((int)HttpStatusCode.InternalServerError, "An internal server error occurred.")
                };

                var rfcTypeUrl = statusCode switch
                {
                    StatusCodes.Status404NotFound => RfcTypeUrls.NotFound,
                    StatusCodes.Status401Unauthorized => RfcTypeUrls.Unauthorized,
                    StatusCodes.Status500InternalServerError => RfcTypeUrls.InternalServerError,
                    _ => RfcTypeUrls.Default
                };

                problemDetails.Title = title;
                problemDetails.Status = statusCode;
                problemDetails.Type = rfcTypeUrl;
                problemDetails.Detail = _env.IsDevelopment() ? exception.StackTrace : null;

                httpContext.Response.StatusCode = statusCode;
            }

            httpContext.Response.ContentType = "application/problem+json";
            var json = JsonSerializer.Serialize(problemDetails);
            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;
        }
    }
}