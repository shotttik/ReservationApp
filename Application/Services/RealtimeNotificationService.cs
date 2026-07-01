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
        private readonly IOutboxMessageRepository _outboxMessageRepository;

        public RealtimeNotificationService(
            INotificationRepository notificationRepository,
            IOutboxMessageRepository outboxMessageRepository)
        {
            _notificationRepository = notificationRepository;
            _outboxMessageRepository = outboxMessageRepository;
        }

        public Task SendToUserAsync(
            int userAccountId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(NotificationTargetType.User, userAccountId, notification, cancellationToken);
        }

        public Task SendToCompanyAsync(
            int companyId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(NotificationTargetType.Company, companyId, notification, cancellationToken);
        }

        public Task SendToBranchAsync(
            int branchId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(NotificationTargetType.Branch, branchId, notification, cancellationToken);
        }

        public Task SendToGuestBookingAsync(
            int bookingId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken = default)
        {
            return CreateAsync(NotificationTargetType.GuestBooking, bookingId, notification, cancellationToken);
        }

        private async Task CreateAsync(
            NotificationTargetType targetType,
            int targetId,
            RealtimeNotificationPayload notification,
            CancellationToken cancellationToken)
        {
            var dataJson = notification.Data == null
                ? null
                : JsonSerializer.Serialize(notification.Data);

            var entity = new Notification
            {
                TargetType = targetType,
                TargetId = targetId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                DataJson = dataJson,
                CreatedAt = notification.CreatedAt
            };

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
                    CreatedAt = notification.CreatedAt
                },
                cancellationToken);
        }
    }
}
