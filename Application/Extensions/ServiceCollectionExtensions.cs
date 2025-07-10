using Application.Common.Security;
using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IWorkScheduleService, WorkScheduleService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICompanyFAQService, CompanyFAQService>();
            services.AddScoped<ICompanyFAQCategoryService, CompanyFAQCategoryService>();
            services.AddScoped<IAccessGuard, AccessGuard>();

            return services;
        }
    }
}
