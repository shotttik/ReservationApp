using API.Middlewares;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddUserAccountDbContext(connectionString);

// Add AutoMapper profiles and application services using the extension methods
builder.Services.AddAutoMapperProfiles();
builder.Services.AddApplicationServices();
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
    // Add any other services here, for example:
    // services.AddDbContext<MyDbContext>();
    // services.AddScoped<IMyService, MyService>();
    // Register DbContext with SQL Server

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
    app.UseAuthorization();

    app.MapControllers();
    app.UseMiddleware<LoggingMiddleware>();
    // Add any other middleware here, for example:
    // app.UseAuthentication();
    // app.UseCors("AllowSpecificOrigins");
}
