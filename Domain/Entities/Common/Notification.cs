using Domain.Enums;

namespace Domain.Entities.Common
{
    public class Notification :BaseEntity
    {
        public NotificationTargetType TargetType { get; set; }
        public int TargetId { get; set; }

        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? DataJson { get; set; }

        public ICollection<NotificationRecipient> Recipients { get; set; } = [];
    }
}
