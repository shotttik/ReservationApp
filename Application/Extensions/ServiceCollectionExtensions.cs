using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add any other services here, for example:
            // services.AddScoped<IMyService, MyService>();
            services.AddScoped<IUserService, UserService>();


            return services;
        }
    }
}
