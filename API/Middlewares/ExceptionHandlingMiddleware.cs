using Application.Common.Results;
using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AuthorizationException ex)
            {
                context.Items ["Exception"] = ex;

                if (context.Response.HasStarted)
                    throw;

                await WriteProblemDetails(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    "https://tools.ietf.org/html/rfc7235#section-3.1",
                    Error.Unauthorized(
                        "Authorization.Unauthorized",
                        "Authentication is required."));
            }
            catch (Exception ex)
            {
                context.Items ["Exception"] = ex;

                if (context.Response.HasStarted)
                    throw;

                await WriteProblemDetails(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Error.Failure(
                        "Server.Failure",
                        ex.Message));
            }
        }

        private static async Task WriteProblemDetails(
            HttpContext context,
            int statusCode,
            string title,
            string type,
            Error error)
        {
            context.Response.Clear();

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Extensions =
                {
                    { "errors", new[] { error } }
                }
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}