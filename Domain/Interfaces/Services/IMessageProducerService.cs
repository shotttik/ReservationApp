namespace Domain.Interfaces.Services
{
    public interface IMessageProducerService
    {
        Task PublishEmailAsync(object payload, CancellationToken cancellationToken = default);
        Task PublishSmsAsync(object payload, CancellationToken cancellationToken = default);
        Task PublishNotificationAsync(object payload, CancellationToken cancellationToken = default);
    }
}
