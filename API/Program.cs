using API.Middlewares;
using Application.Authentication;
using Application.Extensions;
using Application.Options;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(builder.Configuration)
           .CreateLogger();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

// Configure services
ConfigureServices(builder.Services);
ConfigureAuthentication(builder);
ConfigureRateLimit(builder);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add AutoMapper profiles and application services using the extension methods
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(connectionString!);

// Build the app
var app = builder.Build();

// Configure the middleware pipeline
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services)
{
    // Add controllers
    services.AddControllers();

    // Add Swagger/OpenAPI services
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Register DbContext with SQL Server
    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetSection("Redis") ["ConnectionString"];
    });

    services.AddHttpContextAccessor();
    services.AddAuthorization();
    services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
    services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
}

void ConfigureMiddleware(WebApplication app)
{
    // Enable Swagger in development environment
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Other middleware
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseRateLimiter();
    app.MapControllers();
    app.UseMiddleware<LoggingMiddleware>();
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration ["Jwt:Issuer"],
            ValidAudience = builder.Configuration ["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration ["Jwt:Key"]!)),
            ClockSkew = TimeSpan.Zero
        };
    });
}

void ConfigureRateLimit(WebApplicationBuilder builder)
{
    builder.Services.Configure<FixedRateLimitOptions>(
        builder.Configuration.GetSection(FixedRateLimitOptions.FixedRateLimit));

    var fixedOptions = new FixedRateLimitOptions();
    builder.Configuration.GetSection(FixedRateLimitOptions.FixedRateLimit).Bind(fixedOptions);
    var fixedPolicy = "fixed";

    builder.Services.AddRateLimiter(_ =>
    {
        _.AddFixedWindowLimiter(fixedPolicy, options =>
        {
            options.PermitLimit = fixedOptions.PermitLimit;
            options.Window = TimeSpan.FromSeconds(fixedOptions.Window);
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = fixedOptions.QueueLimit;
        });
        _.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        _.OnRejected = async (ctx, cancellationToken) =>
        {
            await ctx.HttpContext.Response.WriteAsync("Request slots exceeded, try again later", cancellationToken);
        };
    });
}
