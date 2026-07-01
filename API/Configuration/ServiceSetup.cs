using Application.Authentication;
using API.Services;
using Application.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Text.Json.Serialization;

namespace API.Configuration
{
    public static class ServiceSetup
    {
        public static IServiceCollection AddConfiguredServices(this IServiceCollection services, IConfiguration config)
        {
            // swagger versioning
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(),
                    new QueryStringApiVersionReader("version"),
                    new HeaderApiVersionReader("X-Version")
                );
            });

            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

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
            services.AddSignalR();
            services.AddScoped<SignalRRealtimeNotificationService>();
            services.AddHostedService<RealtimeNotificationDispatcherService>();
            services.AddHostedService<NotificationOutboxDispatcherService>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, GuestOrUserAuthorizationHandler>();

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
