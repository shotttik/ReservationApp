using Domain.DTO;
using Domain.Entities.Common;
using System.Text.Json;

namespace Application.Extensions.Mappers
{
    public static class NotificationMapper
    {
        public static NotificationDTO MapToDTO(this NotificationRecipient recipient)
        {
            var notification = recipient.Notification;

            return new NotificationDTO
            {
                Id = notification.Id,
                TargetType = notification.TargetType,
                TargetId = notification.TargetId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                Data = string.IsNullOrWhiteSpace(notification.DataJson)
                    ? null
                    : JsonSerializer.Deserialize<object>(notification.DataJson),
                CreatedAt = notification.CreatedAt,
                ReadAt = recipient.ReadAt
            };
        }

    }
}
