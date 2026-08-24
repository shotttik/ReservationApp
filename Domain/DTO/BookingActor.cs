using Domain.Enums;

namespace Domain.DTO
{
    public sealed record BookingActor(
    int? UserId,
    BookingChangeSource Source);

}
