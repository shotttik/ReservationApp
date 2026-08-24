using Domain.DTO;
using Domain.Entities.Common;
using Domain.Enums;

namespace Domain.Interfaces
{
    public interface IBookingHistoryWriter
    {
        Task Add(
            Booking booking,
            ActionType action,
            IReadOnlyCollection<BookingFieldChange> changes,
            BookingActor actor);
    }
}
