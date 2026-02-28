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
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "uploads")),
                RequestPath = "/uploads"
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRateLimiter();
            app.MapControllers();
            app.UseMiddleware<LoggingMiddleware>();

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
