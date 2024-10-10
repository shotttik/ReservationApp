using API.Attributes;
using Application.Common.ResultsErrors;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog.Context;
using Shared.Extensions;
using System.Net;
using ILogger = Serilog.ILogger;

namespace API.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger logger;
        //private AuthUser? AuthUser;
        private LoggingType loggingType;

        public LoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            //var authService = context.RequestServices.GetService<IAuthService>();

            //if (authService != null)
            //{
            //    AuthUser = authService.GetCurrentUser();
            //}
            loggingType = GetLoggingType(context);

            try
            {
                if (loggingType == LoggingType.None)
                {
                    await _next(context);
                }
                else
                {
                    LogRequestDetails(context);

                    HttpRequest request = context.Request;

                    if (request.ContentType != null && request.ContentType.ToLower().Contains("application/json"))
                    {
                        var requestBody = await ReadBodyFromRequest(request);
                        LogContext.PushProperty("RequestBody", requestBody);

                    }
                    else // Log when getting multipart/form-data;
                    {
                        if (loggingType == LoggingType.Full)
                        {
                            LogRequestBody(context);
                        }
                    }

                    await LogResponseDetails(context);


                }


            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
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
            //LogContext.PushProperty("UN_ID", AuthUser?.UnID);
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
                    var responseText = new StreamReader(responseBody).ReadToEnd();
                    LogContext.PushProperty("Response", responseText);

                    responseBody.Seek(0, SeekOrigin.Begin);
                    await responseBody.CopyToAsync(originalBodyStream);

                }
                LogContext.PushProperty("StatusCode", context.Response.StatusCode);
                logger.Information("Success");
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
        private async Task HandleException(HttpContext context, Exception ex)
        {
            var serverErrorCode = (int)HttpStatusCode.InternalServerError;
            var error = Error.Failure("Server.Failure", ex.Message);
            var problemDetails = new ProblemDetails
            {
                Status = serverErrorCode,
                Title = "Internal Server Error",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Extensions = { { "errors", new [] { error } } }
            };
            var errorMessage = JsonConvert.SerializeObject(problemDetails, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            context.Response.StatusCode = serverErrorCode;
            context.Response.ContentType = "application/json";

            if (loggingType != LoggingType.None)
            {
                LogContext.PushProperty("StatusCode", serverErrorCode);
                LogContext.PushProperty("Response", errorMessage);
                logger.Error(ex, "Exception has occurred");
            }

            await context.Response.WriteAsJsonAsync(problemDetails);
        }

        private static LoggingType GetLoggingType(HttpContext context)
        {
            var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
            var loggingType = endpoint?.Metadata.GetMetadata<LoggingAttribute>()?.loggingType;
            loggingType ??= LoggingType.General;

            return (LoggingType)loggingType;
        }

    }

}

