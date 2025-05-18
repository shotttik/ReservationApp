using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Application.Common.Results
{
    public static class ResultExtensions
    {
        public static IActionResult ToResponse(this Result result)
        {
            if (result.IsSuccess)
            {
                return ToSuccessResponse(result);
            }

            return result.ToProblemDetails();
        }

        public static IActionResult ToResponse<TValue>(this Result<TValue> result)
        {
            if (result.IsSuccess)
            {
                return ToSuccessResponse(result, result.Value);
            }

            return result.ToProblemDetails();
        }

        private static IActionResult ToSuccessResponse(Result result)
        {
            var response = new SuccessResponse
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Code = result.SuccessInfo?.Code ?? "Operation.Successful",
                Message = result.SuccessInfo?.Message ?? "Operation completed successfully"
            };

            return new OkObjectResult(response);
        }

        private static IActionResult ToSuccessResponse<TValue>(Result result, TValue value)
        {
            var response = new SuccessResponse<TValue>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Code = result.SuccessInfo?.Code ?? "Operation.Successful",
                Message = result.SuccessInfo?.Message ?? "Operation completed successfully",
                Data = value
            };

            return new OkObjectResult(response);
        }
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
    public class SuccessResponse
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Metadata { get; set; }
    }

    public class SuccessResponse<TValue> :SuccessResponse
    {
        public TValue? Data { get; set; }
    }

}
