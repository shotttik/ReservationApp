using Domain.Enums;

namespace Domain.DTO
{
    public sealed record BookingHistoryDto(
        int Id,
        int BookingId,
        ActionType ActionType,
        int? ChangedByUserId,
        BookingChangeSource Source,
        DateTime CreatedAt,
        IReadOnlyCollection<BookingFieldChange> Changes);
}
