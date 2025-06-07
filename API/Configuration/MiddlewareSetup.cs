using API.Middlewares;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace API.Configuration
{
    public static class MiddlewareSetup
    {
        public static WebApplication UseConfiguredMiddleware(this WebApplication app)
        {
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
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

            if (app.Environment.IsDevelopment())
            {
                await DbSeeder.SeedAsync(db);
            }

            return app;
        }
    }
}
