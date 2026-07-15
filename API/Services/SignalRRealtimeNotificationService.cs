using API.Hubs;
using Application.Common.Notifications;
using Domain.Entities.Common;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace API.Services
{
    public class SignalRRealtimeNotificationService
    {
        private const string NotificationEvent = "notification";
        private readonly IHubContext<NotificationsHub> _hubContext;

        public SignalRRealtimeNotificationService(IHubContext<NotificationsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task SendAsync(Notification notification, CancellationToken cancellationToken = default)
        {
            var payload = new RealtimeNotificationPayload
            {
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                Data = string.IsNullOrWhiteSpace(notification.DataJson)
                    ? null
                    : JsonSerializer.Deserialize<object>(notification.DataJson),
                CreatedAt = notification.CreatedAt
            };

            return _hubContext.Clients
                .Group(GetGroupName(notification))
                .SendAsync(NotificationEvent, payload, cancellationToken);
        }

        private static string GetGroupName(Notification notification)
        {
            return notification.TargetType switch
            {
                NotificationTargetType.User => NotificationGroups.User(notification.TargetId),
                NotificationTargetType.Company => NotificationGroups.Company(notification.TargetId),
                NotificationTargetType.Branch => NotificationGroups.Branch(notification.TargetId),
                //NotificationTargetType.GuestBooking => NotificationGroups.GuestBooking(notification.TargetId),
                _ => throw new InvalidOperationException($"Unsupported notification target type {notification.TargetType}.")
            };
        }
    }
}
