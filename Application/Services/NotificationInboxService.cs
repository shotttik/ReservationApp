using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class NotificationInboxService :INotificationInboxService
    {
        private readonly IAuthService _authService;
        private readonly INotificationRecipientRepository _notificationRecipientRepository;

        public NotificationInboxService(
            IAuthService authService,
            INotificationRecipientRepository notificationRecipientRepository)
        {
            _authService = authService;
            _notificationRecipientRepository = notificationRecipientRepository;
        }

        public async Task<Result<List<NotificationDTO>>> GetMineAsync(
            bool unreadOnly,
            int take,
            CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();
            var safeTake = Math.Clamp(take, 1, 100);

            var recipients = await _notificationRecipientRepository.GetForUserAsync(
                user.UserAccountId,
                unreadOnly,
                safeTake,
                cancellationToken);

            return recipients.Select(e => e.MapToDTO()).ToList();
        }

        public async Task<Result> MarkReadAsync(
            int notificationId,
            CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();

            var recipient = await _notificationRecipientRepository.GetForUserByIdAsync(
                notificationId,
                user.UserAccountId,
                cancellationToken);

            if (recipient == null)
            {
                return Result.Failure(NotificationInboxResults.NotFound);
            }

            await _notificationRecipientRepository.MarkReadAsync(
                recipient,
                cancellationToken);

            return Result.Success(NotificationInboxResults.Read);
        }

        public async Task<Result> MarkReadAllAsync(
            CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();

            await _notificationRecipientRepository.MarkReadAllForUserAsync(
                user.UserAccountId,
                cancellationToken);

            return Result.Success(NotificationInboxResults.ReadAll);
        }

        public async Task<Result> DeleteAsync(
            int notificationId,
            CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();
            var recipient = await _notificationRecipientRepository.GetForUserByIdAsync(
                notificationId,
                user.UserAccountId,
                cancellationToken);
            if (recipient == null)
            {
                return Result.Failure(NotificationInboxResults.NotFound);
            }
            await _notificationRecipientRepository.DisableAsync(
                recipient,
                cancellationToken);
            return Result.Success(NotificationInboxResults.Deleted);
        }

        public async Task<Result> DeleteAllAsync(CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();

            await _notificationRecipientRepository.DisableAllForUserAsync(
                user.UserAccountId,
                cancellationToken);

            return Result.Success(NotificationInboxResults.DeletedAll);
        }
    }
}