using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Jobs
{
    public class EmailNotificationJob :IJob
    {
        private readonly ILogger<EmailNotificationJob> _logger;
        public EmailNotificationJob(ILogger<EmailNotificationJob> logger)
        {
            _logger = logger;

        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Executing Email Notification");
            // Your scheduled logic here
            Console.WriteLine("Email notification sent at: " + DateTime.Now);
            _logger.LogInformation("Email Notification Job completed at");
            await Task.CompletedTask;
        }
    }

}
