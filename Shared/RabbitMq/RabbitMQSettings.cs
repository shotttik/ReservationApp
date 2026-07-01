namespace Shared.RabbitMq
{
    public class RabbitMQSettings
    {
        public required string Host { get; set; }
        public int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string VirtualHost { get; set; }
        public required string ExchangeName { get; set; }
        public required string EmailQueue { get; set; }
        public required string SMSQueue { get; set; }
        public string NotificationQueue { get; set; } = "notification_queue";

        public string Queue(QueueType queue)
        {
            return queue switch
            {
                QueueType.Email => EmailQueue,
                QueueType.SMS => SMSQueue,
                QueueType.Notification => NotificationQueue,
                _ => throw new ArgumentException("Invalid queue type provided", nameof(queue))
            };
        }
        public string RouteKey(QueueType queue)
        {
            return queue switch
            {
                QueueType.Email => EmailQueue.Split("_").First(),
                QueueType.SMS => SMSQueue.Split("_").First(),
                QueueType.Notification => NotificationQueue.Split("_").First(),
                _ => throw new ArgumentException("Invalid queue type provided", nameof(queue))
            };
        }
    }

}
