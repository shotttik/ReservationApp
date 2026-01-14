using Application.Common.Requests.Booking;
using Application.Common.Responses;
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
            string code,
            string bookingReference
            );
        Task<Result> Verify(int bookingId, BookingVerificationRequest request);
        Task<Result> ResendVerificationCode(int bookingId);
        Task<Result> SendGuestAccessCode(GuestBookingAccessRequest request);
        Task<Result<CreateGuestTokenResponse>> VerifyGuestBookingAccess(GuestBookingAccessVerifyRequest request);
    }
}
