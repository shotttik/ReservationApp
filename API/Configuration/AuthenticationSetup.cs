using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Configuration
{
    public static class AuthenticationSetup
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration config)
        {
            var jwtIssuer = GetRequiredConfigValue(config, "Jwt:Issuer");
            var jwtAudience = GetRequiredConfigValue(config, "Jwt:Audience");
            var jwtKey = GetRequiredConfigValue(config, "Jwt:Key");
            var guestJwtKey = GetRequiredConfigValue(config, "BookingSettings:GuestToken:Key");

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
                            ValidIssuer = jwtIssuer,
                            ValidAudience = jwtAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                            ClockSkew = TimeSpan.Zero
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
                                Encoding.UTF8.GetBytes(guestJwtKey)
                            ),
                            ClockSkew = TimeSpan.Zero
                        };
                    });

            return services;
        }

        private static string GetRequiredConfigValue(IConfiguration config, string key)
        {
            var value = config[key];
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            throw new InvalidOperationException($"Missing required configuration value '{key}'.");
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
