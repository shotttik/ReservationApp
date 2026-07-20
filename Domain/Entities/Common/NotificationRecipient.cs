using Domain.Entities.User;
using Domain.Enums;

namespace Domain.Entities.Common
{
    public class NotificationRecipient :ActivableEntity
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; } = null!;

        public int UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; } = null!;


        public DateTime? ReadAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public NotificationStatus DeliveryStatus { get; set; } = NotificationStatus.Pending;

        public int DeliveryAttempts { get; set; }

        public DateTime? LastDeliveryAttemptAt { get; set; }

        public string? LastDeliveryError { get; set; }
    }
}
