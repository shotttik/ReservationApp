using API.Hubs;
using API.Middlewares;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace API.Configuration
{
    public static class MiddlewareSetup
    {
        public static WebApplication UseConfiguredMiddleware(this WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
                RequestPath = "/uploads"
            });

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseMiddleware<LoggingMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

            app.MapControllers();
            app.MapHub<NotificationsHub>("/hubs/notifications");

            return app;
        }

        public static async Task<WebApplication> MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();

            if (!app.Environment.IsProduction())
            {
                await DbSeeder.SeedAsync(db);
            }

            return app;
        }
    }
}
