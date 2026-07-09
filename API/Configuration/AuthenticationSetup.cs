using Application.Common.Results;
using Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Configuration
{
    public static class AuthenticationSetup
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = new JwtOptions();
            configuration.GetSection(JwtOptions.ConfigurationSection).Bind(jwtOptions);

            var bookingOptions = new BookingOptions();
            configuration.GetSection(BookingOptions.ConfigurationSection).Bind(bookingOptions);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Events = CreateSignalRJwtBearerEvents();
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtOptions.Issuer,
                            ValidAudience = jwtOptions.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnChallenge = async context =>
                            {
                                context.HandleResponse();

                                var errorMessage = "Authentication is required.";

                                if (context.AuthenticateFailure is SecurityTokenExpiredException)
                                {
                                    errorMessage = "Access token has expired.";
                                }

                                var problemDetails = new ProblemDetails
                                {
                                    Status = StatusCodes.Status401Unauthorized,
                                    Title = "Unauthorized",
                                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                                    Extensions =
                        {
                        {
                            "errors",
                            new[]
                            {
                                Error.Unauthorized(
                                    "Authorization.Unauthorized",
                                    errorMessage)
                            }
                        }
                        }
                                };

                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/problem+json";

                                await context.Response.WriteAsJsonAsync(problemDetails);
                            }
                        };
                    })
                    .AddJwtBearer("Guest", options =>
                    {
                        options.Events = CreateSignalRJwtBearerEvents();
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,

                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(bookingOptions.GuestToken.Key)
                            ),
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnChallenge = async context =>
                            {
                                context.HandleResponse();

                                var errorMessage = "Authentication is required.";

                                if (context.AuthenticateFailure is SecurityTokenExpiredException)
                                {
                                    errorMessage = "Access token has expired.";
                                }

                                var problemDetails = new ProblemDetails
                                {
                                    Status = StatusCodes.Status401Unauthorized,
                                    Title = "Unauthorized",
                                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                                    Extensions =
                        {
                        {
                            "errors",
                            new[]
                            {
                                Error.Unauthorized(
                                    "Authorization.Unauthorized",
                                    errorMessage)
                            }
                        }
                        }
                                };

                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.ContentType = "application/problem+json";

                                await context.Response.WriteAsJsonAsync(problemDetails);
                            }
                        };

                    });

            return services;
        }

        private static JwtBearerEvents CreateSignalRJwtBearerEvents()
        {
            return new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query ["access_token"];
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs/notifications"))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
        }
    }
}
