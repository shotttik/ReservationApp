using Application.Jobs;
using Infrastructure.Quartz;
using Quartz;

namespace API.Configuration
{
    public static class QuartzSetup
    {
        public static void AddQuartzJobs(this IServiceCollection services, IConfiguration configuration)
        {
            var cron = configuration ["Quartz:EmailNotificationJob:Cron"];

            if (string.IsNullOrWhiteSpace(cron))
                throw new InvalidOperationException("Cron expression for EmailNotificationJob is missing in configuration.");

            services.AddQuartz(q =>
            {
                var jobKey = new JobKey(nameof(EmailNotificationJob));
                q.AddJob<LoggingJobWrapper<EmailNotificationJob>>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobKey + "-trigger")
                .WithCronSchedule(cron));
            });

            services.AddQuartzHostedService(opts => opts.WaitForJobsToComplete = true);
        }
    }
}
