using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface INotificationRecipientRepository
        :IBaseRepository<NotificationRecipient>
    {
        Task<List<NotificationRecipient>> GetPendingForDeliveryAsync(
            int batchSize,
            int maxAttempts,
            CancellationToken cancellationToken);

        Task<List<NotificationRecipient>> GetForUserAsync(
            int userAccountId,
            bool unreadOnly,
            int take,
            CancellationToken cancellationToken);

        Task<NotificationRecipient?> GetForUserByIdAsync(
            int notificationId,
            int userAccountId,
            CancellationToken cancellationToken);

        Task MarkDeliveredAsync(
            NotificationRecipient recipient,
            CancellationToken cancellationToken);

        Task MarkFailedAsync(
            NotificationRecipient recipient,
            string error,
            CancellationToken cancellationToken);

        Task MarkReadAsync(
            NotificationRecipient recipient,
            CancellationToken cancellationToken);

        Task MarkReadAllForUserAsync(
            int userAccountId,
            CancellationToken cancellationToken);

        Task DisableAsync(
            NotificationRecipient recipient,
            CancellationToken cancellationToken);

        Task DisableAllForUserAsync(
            int userAccountId,
            CancellationToken cancellationToken);
    }
}