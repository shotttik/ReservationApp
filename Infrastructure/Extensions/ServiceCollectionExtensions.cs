using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Mappings;
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
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();

            return services;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserAccountProfile));
            return services;
        }


        public static IServiceCollection AddUserAccountDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UserAccountDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

    }
}
