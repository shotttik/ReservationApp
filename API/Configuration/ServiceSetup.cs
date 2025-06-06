using API.Extensions;
using Application.Authentication;
using Application.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace API.Configuration
{
    public static class ServiceSetup
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, IConfiguration config)
        {
            // Controllers + JSON Options + Custom Validation Response
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new CustomValidationProblemDetails(context.ModelState);
                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                });

            // Swagger configuration (moved to extension)
            services.AddEndpointsApiExplorer();
            services.AddSwaggerDocumentation();

            // Redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = config.GetSection("Redis") ["ConnectionString"];
            });

            // Common services
            services.AddHttpContextAccessor();
            services.AddAuthorization();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
