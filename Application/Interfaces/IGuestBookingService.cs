using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Domain.Entities.Common;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IGuestBookingService
    {
        (BookingVerification, string code) CreateBookingVerification(VerificationType verificationType);
        Task<Result> Verify(int bookingId, BookingVerificationRequest request);
        Task<Result> ResendVerificationCode(int bookingId);
        Task<Result> SendGuestAccessCode(GuestBookingAccessRequest request);
        Task<Result<CreateGuestTokenResponse>> VerifyGuestAccess(GuestBookingAccessVerifyRequest request);
        Task<Result> UpdateGuestInfoContact(int routeBookingId, BookingGuestInfoContactUpdateRequest request);
    }
}
