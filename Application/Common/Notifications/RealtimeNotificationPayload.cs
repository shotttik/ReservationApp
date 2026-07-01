namespace Application.Common.Notifications
{
    public class RealtimeNotificationPayload
    {
        public required string Type { get; set; }
        public required string Title { get; set; }
        public required string Message { get; set; }
        public object? Data { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
