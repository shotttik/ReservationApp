using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Configuration
{
    public static class AuthenticationSetup
    {
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services, IConfiguration config)
        {

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
                            ValidIssuer = config ["Jwt:Issuer"],
                            ValidAudience = config ["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config ["Jwt:Key"]!)),
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
                                Encoding.UTF8.GetBytes(config ["BookingSettings:GuestToken:Key"]!)
                            ),
                            ClockSkew = TimeSpan.Zero
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
