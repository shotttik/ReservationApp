using Application.Options;
using Shared.RabbitMq;

namespace API.Configuration
{
    public static class OptionSetup
    {
        public static void ConfigureOptionsSetup(this IServiceCollection services)
        {
            services.AddOptions<RabbitMQSettings>()
                .BindConfiguration(RabbitMQSettings.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<AppUrls>()
                .BindConfiguration(AppUrls.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<BookingOptions>()
                .BindConfiguration(BookingOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<MediaCleanupJobOptions>()
                .BindConfiguration(MediaCleanupJobOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<FixedRateLimitOptions>()
                .BindConfiguration(FixedRateLimitOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<MediaLimitsOptions>()
                .BindConfiguration(MediaLimitsOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<CompanyOptions>()
                .BindConfiguration(CompanyOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddOptions<RedisOptions>()
                .BindConfiguration(RedisOptions.ConfigurationSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}
