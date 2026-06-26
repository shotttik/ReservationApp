using Domain.Entities.Common;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IBookingNotificationService
    {
        Task SendVerificationCodeAsync(
            VerificationType verificationType,
            string contact,
            string? displayName,
            string code,
            Booking booking,
            CancellationToken cancellationToken = default);
    }
}
