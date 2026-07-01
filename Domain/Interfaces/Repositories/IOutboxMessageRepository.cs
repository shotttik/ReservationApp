using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IOutboxMessageRepository :IBaseRepository<OutboxMessage>
    {
        Task<List<OutboxMessage>> GetPendingAsync(int batchSize, int maxAttempts, CancellationToken cancellationToken);
        Task MarkPublishedAsync(OutboxMessage message, CancellationToken cancellationToken);
        Task MarkFailedAsync(OutboxMessage message, string error, CancellationToken cancellationToken);
    }
}
