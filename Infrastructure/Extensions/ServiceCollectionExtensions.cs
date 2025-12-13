using Application.Options;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.EmailTemplates;
using Infrastructure.RabbitMq;
using Infrastructure.Repositories;
using Infrastructure.Services;
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
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ICompanyFAQRepository, CompanyFAQRepository>();
            services.AddScoped<ICompanyFAQCategoryRepository, CompanyFAQCategoryRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<ICompanyMediaRepository, CompanyMediaRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IReviewInviteRepository, ReviewInviteRepository>();
            services.AddScoped<IReviewMediaRepository, ReviewMediaRepository>();

            services.AddSingleton<ICacheService, CacheService>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IImageProcessingService, ImageProcessingService>();
            //@TODO remove comments
            //services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));
            services.Configure<AppUrls>(configuration.GetSection("AppUrls"));
            // email
            //services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<IEmailTemplateBuilder, EmailTemplateBuilder>();
            // rabbitmq
            services.AddSingleton<IMessageProducerService, MessageProducerService>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            return services;
        }
    }
}
