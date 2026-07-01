using Application.Options;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.RabbitMq;


namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserAccountRepository, UserAccountRepository>();
            services.AddScoped<IUserLoginDataRepository, UserLoginDataRepository>();
            services.AddScoped<IUserAccountMediaRepository, UserAccountMediaRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyInvitationRepository, CompanyInvitationRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();
            services.AddScoped<IWorkScheduleExceptionRepository, WorkScheduleExceptionRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICompanyFAQRepository, CompanyFAQRepository>();
            services.AddScoped<ICompanyFAQCategoryRepository, CompanyFAQCategoryRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<ICompanyMediaRepository, CompanyMediaRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingGuestInfoRepository, BookingGuestInfoRepository>();
            services.AddScoped<IBookingVerificationRepository, BookingVerificationRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewInviteRepository, ReviewInviteRepository>();
            services.AddScoped<IReviewMediaRepository, ReviewMediaRepository>();
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<ICompanySubscriptionRepository, CompanySubscriptionRepository>();
            services.AddScoped<IPromoCodeRepository, PromoCodeRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            services.AddScoped<IMediaCleanupService, LocalMediaCleanupService>();
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            services.Configure<AppUrls>(configuration.GetSection("AppUrls"));
            services.AddOptions<BookingSettings>().BindConfiguration(BookingSettings.ConfigurationSection);
            services.AddOptions<MediaCleanupJobSettings>().BindConfiguration(MediaCleanupJobSettings.ConfigurationSection);
            // email
            services.AddSingleton<IEmailTemplateBuilder, EmailTemplateBuilder>();
            services.AddSingleton<ISmsTemplateBuilder, SmsTemplateBuilder>();
            // rabbitmq
            services.AddSingleton<IMessageProducerService, MessageProducerService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            return services;
        }
    }
}
