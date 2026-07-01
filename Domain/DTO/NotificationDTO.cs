using Domain.Enums;

namespace Domain.DTO
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public NotificationTargetType TargetType { get; set; }
        public int TargetId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
