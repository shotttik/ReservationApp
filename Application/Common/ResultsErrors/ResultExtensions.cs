﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Common.ResultsErrors
{
    public static class ResultExtensions
    {
        public static IActionResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            var problemDetails = new ProblemDetails
            {
                Status = GetStatusCode(result.Error.Type),
                Title = GetTitle(result.Error.Type),
                Type = GetType(result.Error.Type),
                Extensions = { { "errors", new [] { result.Error } } }
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };

            static int GetStatusCode(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                    ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status500InternalServerError
                };

            static string GetTitle(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => "Bad Request",
                    ErrorType.NotFound => "Not Found",
                    ErrorType.Conflict => "Conflict",
                    ErrorType.Unauthorized => "Unauthorized",
                    ErrorType.Forbidden => "Forbidden",
                    _ => "Server Failure"
                };
            static string GetType(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
                    ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                    _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };
        }
    }
}
