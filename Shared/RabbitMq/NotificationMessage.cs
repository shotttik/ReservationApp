namespace Shared.RabbitMq
{
    public class NotificationMessage
    {
        public int NotificationId { get; set; }
        public required string TargetType { get; set; }
        public int TargetId { get; set; }
        public required string Type { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public string? DataJson { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
