using Application.Common.Results;
using Application.Interfaces;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using System.Text.Json;

namespace Application.Services
{
    public class NotificationInboxService :INotificationInboxService
    {
        private readonly IAuthService _authService;
        private readonly INotificationRepository _notificationRepository;

        public NotificationInboxService(
            IAuthService authService,
            INotificationRepository notificationRepository)
        {
            _authService = authService;
            _notificationRepository = notificationRepository;
        }

        public async Task<Result<List<NotificationDTO>>> GetMineAsync(
            bool unreadOnly,
            int take,
            CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();
            var safeTake = Math.Clamp(take, 1, 100);

            var notifications = await _notificationRepository.GetForUserAsync(
                user.UserAccountId,
                user.CompanyId,
                user.BranchId,
                unreadOnly,
                safeTake,
                cancellationToken);

            return notifications.Select(MapToDTO).ToList();
        }

        public async Task<Result> MarkReadAsync(int notificationId, CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();
            var notification = await _notificationRepository.GetForUserByIdAsync(
                notificationId,
                user.UserAccountId,
                user.CompanyId,
                user.BranchId,
                cancellationToken);

            if (notification == null)
            {
                return Result.Failure(Error.NotFound("Notification.NotFound", "Notification not found."));
            }

            await _notificationRepository.MarkReadAsync(notification, cancellationToken);

            return Result.Success("Notification.Read", "Notification marked as read.");
        }

        public async Task<Result> MarkReadAllAsync(CancellationToken cancellationToken)
        {
            var user = await _authService.GetCurrentUser();
            await _notificationRepository.MarkReadAllForUserAsync(
                user.UserAccountId,
                user.CompanyId,
                user.BranchId,
                cancellationToken);
            return Result.Success("Notification.ReadAll", "All notifications marked as read.");
        }
        private static NotificationDTO MapToDTO(Notification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                TargetType = notification.TargetType,
                TargetId = notification.TargetId,
                Type = notification.Type,
                Title = notification.Title,
                Message = notification.Message,
                Data = string.IsNullOrWhiteSpace(notification.DataJson)
                    ? null
                    : JsonSerializer.Deserialize<object>(notification.DataJson),
                CreatedAt = notification.CreatedAt,
                ReadAt = notification.ReadAt
            };
        }
    }
}
