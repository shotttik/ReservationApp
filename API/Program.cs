using API.Configuration;
using Application.Extensions;
using Infrastructure.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Configurations
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration.GetConnectionString("DefaultConnection")!);
builder.Services.AddConfiguredServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddRateLimitingServices(builder.Configuration);
builder.Services.AddQuartzJobs(builder.Configuration);

var app = builder.Build();
await app.MigrateAndSeedAsync();

app.UseConfiguredMiddleware();
app.UseSwaggerDocumentation();
app.Run();
