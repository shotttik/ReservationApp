using Domain.DTO;
using Domain.Entities.Common;
using System.Text.Json;

namespace Application.Extensions.Mappers
{
    public static class BookingHistoryMapper
    {
        public static BookingHistoryDto HistoryMapToEntity(this BookingHistory history)
        {
            var changes = JsonSerializer.Deserialize<List<BookingFieldChange>>(history.ChangesJson)
                ?? [];

            return new BookingHistoryDto(
                history.Id,
                history.BookingId,
                history.ActionType,
                history.ChangedByUserId,
                history.Source,
                history.CreatedAt,
                changes);
        }
    }
}
