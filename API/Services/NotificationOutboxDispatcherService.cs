using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Shared.RabbitMq;
using System.Text.Json;

namespace API.Services
{
    public class NotificationOutboxDispatcherService :BackgroundService
    {
        private const int BatchSize = 50;
        private const int MaxAttempts = 10;
        private static readonly TimeSpan PollDelay = TimeSpan.FromSeconds(5);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NotificationOutboxDispatcherService> _logger;

        public NotificationOutboxDispatcherService(
            IServiceScopeFactory scopeFactory,
            ILogger<NotificationOutboxDispatcherService> logger)
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
                    _logger.LogError(ex, "Unhandled error while publishing notification outbox messages.");
                }

                await Task.Delay(PollDelay, stoppingToken);
            }
        }

        private async Task DispatchBatchAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
            var producer = scope.ServiceProvider.GetRequiredService<IMessageProducerService>();

            var messages = await repository.GetPendingAsync(BatchSize, MaxAttempts, cancellationToken);

            foreach (var message in messages)
            {
                try
                {
                    var payload = JsonSerializer.Deserialize<NotificationMessage>(message.PayloadJson)
                        ?? throw new InvalidOperationException("Outbox notification payload could not be deserialized.");

                    await producer.PublishNotificationAsync(payload, cancellationToken);
                    await repository.MarkPublishedAsync(message, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(
                        ex,
                        "Failed to publish outbox message {OutboxMessageId}.",
                        message.Id);

                    await repository.MarkFailedAsync(message, ex.Message, cancellationToken);
                }
            }
        }
    }
}
