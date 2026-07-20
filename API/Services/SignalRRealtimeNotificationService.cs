using API.Hubs;
using Domain.Entities.Common;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace API.Services
{
    public class SignalRRealtimeNotificationService
    {
        private const string NotificationEvent = "notification";

        private readonly IHubContext<NotificationsHub> _hubContext;

        public SignalRRealtimeNotificationService(
            IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendAsync(
            NotificationRecipient recipient,
            CancellationToken cancellationToken = default)
        {
            var notification = recipient.Notification;

            var payload = new
            {
                id = notification.Id,
                targetType = notification.TargetType,
                targetId = notification.TargetId,
                type = notification.Type,
                title = notification.Title,
                message = notification.Message,
                data = string.IsNullOrWhiteSpace(notification.DataJson)
                    ? null
                    : JsonSerializer.Deserialize<object>(notification.DataJson),
                createdAt = notification.CreatedAt,
                readAt = recipient.ReadAt
            };

            return _hubContext.Clients
                .Group(NotificationGroups.User(recipient.UserAccountId))
                .SendAsync(NotificationEvent, payload, cancellationToken);
        }
    }
}