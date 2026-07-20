using Application.Common.Results;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface INotificationInboxService
    {
        Task<Result<List<NotificationDTO>>> GetMineAsync(bool unreadOnly, int take, CancellationToken cancellationToken);
        Task<Result> MarkReadAsync(int notificationId, CancellationToken cancellationToken);
        Task<Result> MarkReadAllAsync(CancellationToken cancellationToken);
        Task<Result> DeleteAsync(int notificationId, CancellationToken cancellationToken);
        Task<Result> DeleteAllAsync(CancellationToken cancellationToken);
    }
}
