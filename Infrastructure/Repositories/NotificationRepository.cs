using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NotificationRepository :BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Notification>> GetPendingForDeliveryAsync(
            int batchSize,
            int maxAttempts,
            CancellationToken cancellationToken)
        {
            return await _dbSet
                .Where(e =>
                    (e.DeliveryStatus == NotificationStatus.Pending ||
                     e.DeliveryStatus == NotificationStatus.Failed) &&
                    e.DeliveryAttempts < maxAttempts)
                .OrderBy(e => e.CreatedAt)
                .Take(batchSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Notification>> GetForUserAsync(
            int userAccountId,
            int? companyId,
            int? branchId,
            bool unreadOnly,
            int take,
            CancellationToken cancellationToken)
        {
            var query = ForUserQuery(userAccountId, companyId, branchId);

            if (unreadOnly)
            {
                query = query.Where(e => e.ReadAt == null);
            }

            return await query
                .OrderByDescending(e => e.CreatedAt)
                .Take(take)
                .ToListAsync(cancellationToken);
        }

        public async Task<Notification?> GetForUserByIdAsync(
            int notificationId,
            int userAccountId,
            int? companyId,
            int? branchId,
            CancellationToken cancellationToken)
        {
            return await ForUserQuery(userAccountId, companyId, branchId)
                .FirstOrDefaultAsync(e => e.Id == notificationId, cancellationToken);
        }

        public async Task MarkDeliveredAsync(Notification notification, CancellationToken cancellationToken)
        {
            notification.DeliveryStatus = NotificationStatus.Delivered;
            notification.DeliveredAt = DateTime.UtcNow;
            notification.LastDeliveryError = null;
            notification.UpdateTimestamp();
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkFailedAsync(Notification notification, string error, CancellationToken cancellationToken)
        {
            notification.DeliveryStatus = NotificationStatus.Failed;
            notification.DeliveryAttempts++;
            notification.LastDeliveryAttemptAt = DateTime.UtcNow;
            notification.LastDeliveryError = error.Length > 2000 ? error [..2000] : error;
            notification.UpdateTimestamp();
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkReadAsync(Notification notification, CancellationToken cancellationToken)
        {
            notification.ReadAt ??= DateTime.UtcNow;
            notification.UpdateTimestamp();
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task MarkReadAllForUserAsync(
            int userAccountId,
            int? companyId,
            int? branchId,
            CancellationToken cancellationToken)
        {
            var notifications = await ForUserQuery(userAccountId, companyId, branchId)
                .Where(e => e.ReadAt == null)
                .ExecuteUpdateAsync(e => e.SetProperty(n => n.ReadAt, n => DateTime.UtcNow)
                    .SetProperty(n => n.UpdatedAt, n => DateTime.UtcNow), cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Notification> ForUserQuery(int userAccountId, int? companyId, int? branchId)
        {
            return _dbSet.Where(e =>
                (e.TargetType == NotificationTargetType.User && e.TargetId == userAccountId) ||
                (companyId.HasValue && e.TargetType == NotificationTargetType.Company && e.TargetId == companyId.Value) ||
                (branchId.HasValue && e.TargetType == NotificationTargetType.Branch && e.TargetId == branchId.Value));
        }
    }
}
