using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OutboxMessageRepository :BaseRepository<OutboxMessage>, IOutboxMessageRepository
    {
        public OutboxMessageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<OutboxMessage>> GetPendingAsync(
            int batchSize,
            int maxAttempts,
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(e =>
                    (e.Status == OutboxMessageStatus.Pending ||
                     e.Status == OutboxMessageStatus.Failed) &&
                    e.Attempts < maxAttempts)
                .OrderBy(e => e.CreatedAt)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

        public async Task MarkPublishedAsync(OutboxMessage message, CancellationToken cancellationToken)
        {
            message.Status = OutboxMessageStatus.Published;
            message.PublishedAt = DateTime.UtcNow;
            message.LastError = null;
            message.UpdateTimestamp();
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkFailedAsync(OutboxMessage message, string error, CancellationToken cancellationToken)
        {
            message.Status = OutboxMessageStatus.Failed;
            message.Attempts++;
            message.LastAttemptAt = DateTime.UtcNow;
            message.LastError = error.Length > 2000 ? error [..2000] : error;
            message.UpdateTimestamp();
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
