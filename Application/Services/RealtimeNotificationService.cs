using Application.Common.Notifications;
using Application.Interfaces;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Shared.RabbitMq;
using System.Text.Json;

namespace Application.Services
{
    public class RealtimeNotificationService :IRealtimeNotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IOutboxMessageRepository _outboxMessageRepository;

        public RealtimeNotificationService(
            INotificationRepository notificationRepository,
            IUserAccountRepository userAccountRepository,
            IOutboxMessageRepository outboxMessageRepository)
        {
            _notificationRepository = notificationRepository;
            _userAccountRepository = userAccountRepository;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public Task SendToUserAsync(
            int userAccountId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(
                NotificationTargetType.User,
                userAccountId,
                notification,
                cancellationToken);
        }

        public Task SendToCompanyAsync(
            int companyId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(
                NotificationTargetType.Company,
                companyId,
                notification,
                cancellationToken);
        }

        public Task SendToBranchAsync(
            int branchId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(
                NotificationTargetType.Branch,
                branchId,
                notification,
                cancellationToken);
        }

        private async Task CreateAsync(
            NotificationTargetType targetType,
            int targetId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken)
        {
            var createdAt = notification.CreatedAt == default
                ? DateTime.UtcNow
                : notification.CreatedAt;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var dataJson = notification.Data == null
                ? null
                : JsonSerializer.Serialize(notification.Data, options);

            var recipientUserIds = await GetRecipientUserIdsAsync(
                targetType,
                targetId,
                cancellationToken);

            recipientUserIds = recipientUserIds
                .Distinct()
                .ToList();

            if (recipientUserIds.Count == 0)
            {
                return;
            }

            var entity = new Notification
            {
                TargetType = targetType,
                TargetId = targetId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                DataJson = dataJson,
                CreatedAt = createdAt
            };

            foreach (var userAccountId in recipientUserIds)
            {
                entity.Recipients.Add(new NotificationRecipient
                {
                    UserAccountId = userAccountId,
                    DeliveryStatus = NotificationStatus.Pending,
                    CreatedAt = createdAt
                });
            }

            await _notificationRepository.Add(entity, cancellationToken);

            var message = new NotificationMessage
            {
                NotificationId = entity.Id,
                TargetType = targetType.ToString(),
                TargetId = targetId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                DataJson = dataJson,
                CreatedAt = entity.CreatedAt
            };

            await _outboxMessageRepository.Add(
                new OutboxMessage
                {
                    Type = notification.Type,
                    PayloadJson = JsonSerializer.Serialize(message),
                    CreatedAt = createdAt
                },
                cancellationToken);
        }

        private async Task<List<int>> GetRecipientUserIdsAsync(
            NotificationTargetType targetType,
            int targetId,
            CancellationToken cancellationToken)
        {
            return targetType switch
            {
                NotificationTargetType.User =>
                    new List<int> { targetId },

                NotificationTargetType.Company =>
                    await _userAccountRepository.GetActiveUserAccountIdsByCompanyIdAsync(
                        targetId,
                        cancellationToken),

                NotificationTargetType.Branch =>
                    await _userAccountRepository.GetActiveUserAccountIdsByBranchIdAsync(
                        targetId,
                        cancellationToken),

                _ => throw new InvalidOperationException(
                    $"Unsupported notification target type {targetType}.")
            };
        }
    }
}