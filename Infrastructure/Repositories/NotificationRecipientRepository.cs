namespace Infrastructure.Repositories
{
    using Domain.Entities.Common;
    using Domain.Enums;
    using Domain.Interfaces.Repositories;
    using Microsoft.EntityFrameworkCore;

    namespace Infrastructure.Repositories
    {
        public class NotificationRecipientRepository
            :BaseRepository<NotificationRecipient>, INotificationRecipientRepository
        {
            public NotificationRecipientRepository(ApplicationDbContext dbContext)
                : base(dbContext)
            {
            }

            public async Task<List<NotificationRecipient>> GetPendingForDeliveryAsync(
                int batchSize,
                int maxAttempts,
                CancellationToken cancellationToken)
            {
                return await _dbSet
                    .Include(e => e.Notification)
                    .Where(e =>
                        e.DeletedAt == null &&
                        (e.DeliveryStatus == NotificationStatus.Pending ||
                         e.DeliveryStatus == NotificationStatus.Failed) &&
                        e.DeliveryAttempts < maxAttempts)
                    .OrderBy(e => e.CreatedAt)
                    .Take(batchSize)
                    .ToListAsync(cancellationToken);
            }

            public async Task<List<NotificationRecipient>> GetForUserAsync(
                int userAccountId,
                bool unreadOnly,
                int take,
                CancellationToken cancellationToken)
            {
                var query = _dbSet
                    .Include(e => e.Notification)
                    .Where(e =>
                        e.UserAccountId == userAccountId &&
                        e.ActiveStatus == ActiveStatus.Active &&
                        e.DeletedAt == null);

                if (unreadOnly)
                {
                    query = query.Where(e => e.ReadAt == null);
                }

                return await query
                    .OrderByDescending(e => e.Notification.CreatedAt)
                    .Take(take)
                    .ToListAsync(cancellationToken);
            }

            public async Task<NotificationRecipient?> GetForUserByIdAsync(
                int notificationId,
                int userAccountId,
                CancellationToken cancellationToken)
            {
                return await _dbSet
                    .Include(e => e.Notification)
                    .FirstOrDefaultAsync(e =>
                        e.NotificationId == notificationId &&
                        e.UserAccountId == userAccountId &&
                        e.DeletedAt == null &&
                        e.ActiveStatus == ActiveStatus.Active,
                        cancellationToken);
            }

            public async Task MarkDeliveredAsync(
                NotificationRecipient recipient,
                CancellationToken cancellationToken)
            {
                recipient.DeliveryStatus = NotificationStatus.Delivered;
                recipient.DeliveredAt = DateTime.UtcNow;
                recipient.LastDeliveryError = null;
                recipient.UpdateTimestamp();

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            public async Task MarkFailedAsync(
                NotificationRecipient recipient,
                string error,
                CancellationToken cancellationToken)
            {
                recipient.DeliveryStatus = NotificationStatus.Failed;
                recipient.DeliveryAttempts++;
                recipient.LastDeliveryAttemptAt = DateTime.UtcNow;
                recipient.LastDeliveryError = error.Length > 2000
                    ? error [..2000]
                    : error;

                recipient.UpdateTimestamp();

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            public async Task MarkReadAsync(
                NotificationRecipient recipient,
                CancellationToken cancellationToken)
            {
                recipient.ReadAt ??= DateTime.UtcNow;
                recipient.UpdateTimestamp();

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            public async Task MarkReadAllForUserAsync(
                int userAccountId,
                CancellationToken cancellationToken)
            {
                await _dbSet
                    .Where(e =>
                        e.UserAccountId == userAccountId &&
                        e.ReadAt == null &&
                        e.DeletedAt == null)
                    .ExecuteUpdateAsync(e => e
                        .SetProperty(n => n.ReadAt, DateTime.UtcNow)
                        .SetProperty(n => n.UpdatedAt, DateTime.UtcNow),
                        cancellationToken);
            }

            public async Task DisableAsync(
                NotificationRecipient recipient,
                CancellationToken cancellationToken)
            {
                recipient.Disable();
                await dbContext.SaveChangesAsync(cancellationToken);
            }

            public async Task DisableAllForUserAsync(
                int userAccountId,
                CancellationToken cancellationToken)
            {
                await _dbSet
                    .Where(e =>
                        e.UserAccountId == userAccountId &&
                        e.DeletedAt == null &&
                        e.ActiveStatus == ActiveStatus.Active)
                    .ExecuteUpdateAsync(e => e
                        .SetProperty(n => n.DeletedAt, DateTime.UtcNow)
                        .SetProperty(n => n.ActiveStatus, ActiveStatus.Disabled)
                        .SetProperty(n => n.UpdatedAt, DateTime.UtcNow),
                        cancellationToken);
            }
        }
    }
}
