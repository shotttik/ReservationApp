using API.Attributes;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;
using Serilog.Context;
using Shared.Extensions;

namespace API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> logger;
        private LoggingType loggingType;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            LogContext.PushProperty("LogTarget", "HTTP");

            loggingType = GetLoggingType(context);

            if (loggingType == LoggingType.None)
            {
                await _next(context);
                return;
            }

            LogRequestDetails(context);

            await LogRequestBodyByLoggingType(context);

            await LogResponseDetails(context);
        }

        private async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            // Ensure the request's body can be read multiple times 
            // (for the next middlewares in the pipeline).
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            var requestBody = await streamReader.ReadToEndAsync();
            // Reset the request's body stream position for 
            // next middleware in the pipeline.
            request.Body.Position = 0;

            return requestBody;
        }
        private void LogRequestDetails(HttpContext context)
        {
            LogContext.PushProperty("Method", context.Request.Method);
            LogContext.PushProperty("RequestPath", context.Request.Path);
            LogContext.PushProperty("IpAddress", context.Connection.RemoteIpAddress);
            LogRequestHeaders(context.Request);

            if (loggingType != LoggingType.General)
            {
                LogQueryParameters(context.Request);
            }
        }


        private void LogRequestHeaders(HttpRequest request)
        {
            var headers = request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
            var headersJson = JsonConvert.SerializeObject(headers);
            if (!headers.IsNullOrEmpty())
            {
                LogContext.PushProperty("RequestHeaders", headersJson);
            }
        }

        private void LogQueryParameters(HttpRequest request)
        {
            var queryParameters = request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            var queryParametersJson = JsonConvert.SerializeObject(queryParameters);
            if (!queryParameters.IsNullOrEmpty())
            {
                LogContext.PushProperty("QueryParameters", queryParametersJson);
            }
        }
        private void LogRequestBody(HttpContext context)
        {
            if (context.Request.ContentLength > 0)
            {
                context.Request.EnableBuffering(); // Enable reading the request body more than once
                var requestBody = ParseRequestForm(context.Request.Form);
                LogContext.PushProperty("RequestBody", requestBody);
                context.Request.Body.Seek(0, SeekOrigin.Begin); // Reset the request body stream
            }
        }

        private static string ParseRequestForm(IFormCollection form)
        {

            // Handle form data or other content types as needed
            var file = form.Files.Count > 0 ? form.Files [0] : null;

            var dict = new Dictionary<string, string> { };
            if (file != null)
            {
                var content = new StreamReader(file.OpenReadStream()).ReadToEndAsync();
                dict.Add("xmlFile", content.Result);
            }
            foreach (var formItem in form)
            {
                dict.Add(formItem.Key, formItem.Value!);
            }

            // Convert form data to JSON or handle as needed
            return JsonConvert.SerializeObject(dict);

        }

        private async Task LogResponseDetails(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            try
            {
                using var responseBody = new MemoryStream();

                if (loggingType != LoggingType.General)
                {
                    context.Response.Body = responseBody;
                }

                await _next(context);

                if (loggingType != LoggingType.General)
                {
                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseText = await new StreamReader(responseBody).ReadToEndAsync();

                    LogContext.PushProperty("Response", responseText);

                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);
                }

                LogContext.PushProperty("StatusCode", context.Response.StatusCode);

                if (context.Response.StatusCode >= 500)
                {
                    if (context.Items.TryGetValue("Exception", out var value) &&
                        value is Exception ex)
                    {
                        logger.LogError(ex, "Exception has occurred");
                    }
                    else
                    {
                        logger.LogError("Server failure");
                    }
                }
                else if (context.Response.StatusCode >= 400)
                {
                    logger.LogWarning("Request finished with client error");
                }
                else
                {
                    logger.LogInformation("Success");
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }

        private static LoggingType GetLoggingType(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var loggingType = endpoint?.Metadata.GetMetadata<LoggingAttribute>()?.loggingType;
            loggingType ??= LoggingType.General;

            return (LoggingType)loggingType;
        }
        private async Task LogRequestBodyByLoggingType(HttpContext context)
        {
            if (loggingType != LoggingType.Full)
                return;

            HttpRequest request = context.Request;

            if (request.ContentType != null &&
                request.ContentType.ToLower().Contains("application/json"))
            {
                var requestBody = await ReadBodyFromRequest(request);
                LogContext.PushProperty("RequestBody", requestBody);
            }
            else // Log when getting multipart/form-data
            {
                LogRequestBody(context);
            }
        }
    }

}

