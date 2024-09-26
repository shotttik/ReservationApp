using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add any other services here, for example:
            // services.AddDbContext<MyDbContext>();
            // services.AddScoped<IMyService, MyService>();
            // Register DbContext with SQL Server
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IUserLoginDataRepository, UserLoginDataRepository>();
            return services;
        }

        public static IServiceCollection AddUserDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

    }
}
