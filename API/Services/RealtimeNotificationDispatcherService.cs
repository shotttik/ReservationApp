using Domain.Interfaces.Repositories;

namespace API.Services
{
    public class RealtimeNotificationDispatcherService :BackgroundService
    {
        private const int BatchSize = 50;
        private const int MaxAttempts = 10;
        private static readonly TimeSpan PollDelay = TimeSpan.FromSeconds(5);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<RealtimeNotificationDispatcherService> _logger;

        public RealtimeNotificationDispatcherService(
            IServiceScopeFactory scopeFactory,
            ILogger<RealtimeNotificationDispatcherService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await DispatchBatchAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Unhandled error while dispatching realtime notifications.");
                }

                await Task.Delay(PollDelay, stoppingToken);
            }
        }

        private async Task DispatchBatchAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var recipientRepository = scope.ServiceProvider
                .GetRequiredService<INotificationRecipientRepository>();

            var signalR = scope.ServiceProvider
                .GetRequiredService<SignalRRealtimeNotificationService>();

            var recipients = await recipientRepository.GetPendingForDeliveryAsync(
                BatchSize,
                MaxAttempts,
                cancellationToken);

            foreach (var recipient in recipients)
            {
                try
                {
                    await signalR.SendAsync(recipient, cancellationToken);

                    await recipientRepository.MarkDeliveredAsync(
                        recipient,
                        cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Failed to deliver realtime notification {NotificationId} to user {UserAccountId}.",
                        recipient.NotificationId,
                        recipient.UserAccountId);

                    await recipientRepository.MarkFailedAsync(
                        recipient,
                        ex.Message,
                        cancellationToken);
                }
            }
        }
    }
}