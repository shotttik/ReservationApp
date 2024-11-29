using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // Add any other services here, for example:
            // services.AddDbContext<MyDbContext>();
            // Register DbContext with SQL Server
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IUserLoginDataRepository, UserLoginDataRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
