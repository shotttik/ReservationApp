using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog.Context;

namespace Infrastructure.Quartz
{
    public class LoggingJobWrapper<T> :IJob where T : IJob
    {
        private readonly IServiceProvider provider;

        public LoggingJobWrapper(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var jobName = typeof(T).Name;
            var correlationId = Guid.NewGuid().ToString();
            using (LogContext.PushProperty("LogTarget", "Job"))
            using (LogContext.PushProperty("JobName", jobName))
            using (LogContext.PushProperty("CorrelationId", correlationId))
            {
                var job = provider.GetRequiredService<T>();
                await job.Execute(context);
            }
        }
    }
}
