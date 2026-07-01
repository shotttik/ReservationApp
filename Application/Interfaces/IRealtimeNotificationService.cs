using Application.Common.Notifications;

namespace Application.Interfaces
{
    public interface IRealtimeNotificationService
    {
        Task SendToUserAsync(int userAccountId, RealtimeNotificationPayload notification, CancellationToken cancellationToken = default);
        Task SendToCompanyAsync(int companyId, RealtimeNotificationPayload notification, CancellationToken cancellationToken = default);
        Task SendToBranchAsync(int branchId, RealtimeNotificationPayload notification, CancellationToken cancellationToken = default);
        Task SendToGuestBookingAsync(int bookingId, RealtimeNotificationPayload notification, CancellationToken cancellationToken = default);
    }
}
