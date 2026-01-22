using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Application.Jobs
{
    public class MediaCleanupJob :IJob
    {
        private readonly ILogger<MediaCleanupJob> _logger;
        private readonly IMediaCleanupService _mediaCleanupService;

        public MediaCleanupJob(
            ILogger<MediaCleanupJob> logger,
            IMediaCleanupService mediaCleanupService)
        {
            _logger = logger;
            _mediaCleanupService = mediaCleanupService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Executing Media Cleanup Job");

            await _mediaCleanupService.CleanupOrphanedMediaAsync();

            _logger.LogInformation("Media Cleanup Job completed");

            await Task.CompletedTask;
        }
    }

}
