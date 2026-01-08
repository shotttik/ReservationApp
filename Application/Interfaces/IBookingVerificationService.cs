using Application.Common.Results;
using Domain.Entities.Common;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IBookingVerificationService
    {
        (BookingVerification, string code) CreateBookingVerification(VerificationType verificationType);
        Task SendVerificationNotification(
            VerificationType verificationType,
            string contact,
            string? displayName,
            string code
            );
        Task<Result> SendGuestVerification(int bookingId);
        Task<Result> Verify(int bookingId, string code);
        Task<Result> ResendCode(int bookingId);
    }
}
