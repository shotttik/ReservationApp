using Domain.Enums;

namespace Domain.Entities.Common
{
    public class OutboxMessage :BaseEntity
    {
        public string Type { get; set; } = string.Empty;
        public string PayloadJson { get; set; } = string.Empty;
        public OutboxMessageStatus Status { get; set; } = OutboxMessageStatus.Pending;
        public int Attempts { get; set; }
        public DateTime? LastAttemptAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public string? LastError { get; set; }
    }
}
