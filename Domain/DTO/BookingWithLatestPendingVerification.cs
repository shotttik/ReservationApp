using Domain.Entities.Common;

namespace Domain.DTO
{
    public sealed record BookingWithLatestPendingVerification(
    Booking Booking,
    BookingVerification? LatestPendingVerification
);

}
