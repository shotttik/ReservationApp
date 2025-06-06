using Application.Options;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace API.Configuration
{
    public static class RateLimitingSetup
    {
        public static IServiceCollection AddRateLimitingServices(this IServiceCollection services, IConfiguration config)
        {
            var fixedOptions = new FixedRateLimitOptions();
            config.GetSection(FixedRateLimitOptions.FixedRateLimit).Bind(fixedOptions);

            services.Configure<FixedRateLimitOptions>(config.GetSection(FixedRateLimitOptions.FixedRateLimit));

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("fixed", limiterOptions =>
                {
                    limiterOptions.PermitLimit = fixedOptions.PermitLimit;
                    limiterOptions.Window = TimeSpan.FromSeconds(fixedOptions.Window);
                    limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    limiterOptions.QueueLimit = fixedOptions.QueueLimit;
                });

                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.OnRejected = async (context, token) =>
                {
                    await context.HttpContext.Response.WriteAsync("Request slots exceeded, try again later", token);
                };
            });

            return services;
        }
    }
}
