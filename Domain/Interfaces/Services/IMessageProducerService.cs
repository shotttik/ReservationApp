namespace Infrastructure.RabbitMq
{
    public interface IMessageProducerService
    {
        Task PublishEmailAsync(object payload, CancellationToken cancellationToken = default);
        Task PublishSmsAsync(object payload, CancellationToken cancellationToken = default);
    }
}
