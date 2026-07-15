using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface INotificationRepository :IBaseRepository<Notification>
    {
        Task<List<Notification>> GetPendingForDeliveryAsync(int batchSize, int maxAttempts, CancellationToken cancellationToken);
        Task<List<Notification>> GetForUserAsync(int userAccountId, int? companyId, int? branchId, bool unreadOnly, int take, CancellationToken cancellationToken);
        Task<Notification?> GetForUserByIdAsync(int notificationId, int userAccountId, int? companyId, int? branchId, CancellationToken cancellationToken);
        Task MarkDeliveredAsync(Notification notification, CancellationToken cancellationToken);
        Task MarkFailedAsync(Notification notification, string error, CancellationToken cancellationToken);
        Task MarkReadAsync(Notification notification, CancellationToken cancellationToken);
        Task MarkReadAllForUserAsync(int userAccountId, int? companyId, int? branchId, CancellationToken cancellationToken);
    }
}
