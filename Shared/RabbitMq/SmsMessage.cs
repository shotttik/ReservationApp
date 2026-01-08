namespace Shared.RabbitMq
{
    public class SmsMessage
    {
        public required string ToNumber { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
