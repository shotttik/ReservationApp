using Application.Common.Security;
using Application.Interfaces;
using Application.Jobs;
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
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<ICompanyFAQService, CompanyFAQService>();
            services.AddScoped<ICompanyFAQCategoryService, CompanyFAQCategoryService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IGuestBookingService, GuestBookingService>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();

            services.AddScoped<IAccessGuard, AccessGuard>();
            services.AddScoped<ISubscriptionGuard, SubscriptionGuard>();

            services.AddScoped<MediaCleanupJob>();


            return services;
        }
    }
}
