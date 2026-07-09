using Application.Jobs;
using Application.Options;
using Infrastructure.Quartz;
using Quartz;

namespace API.Configuration
{
    public static class QuartzSetup
    {
        public static void AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var fixedOptions = new MediaCleanupJobOptions();
            configuration.GetSection(MediaCleanupJobOptions.ConfigurationSection).Bind(fixedOptions);

            services.AddQuartz(q =>
            {
                var jobKey = new JobKey(nameof(MediaCleanupJob));
                q.AddJob<LoggingJobWrapper<MediaCleanupJob>>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobKey + "-trigger")
                .WithCronSchedule(fixedOptions.Cron));
            });

            services.AddQuartzHostedService(opts => opts.WaitForJobsToComplete = true);
        }
    }
}
