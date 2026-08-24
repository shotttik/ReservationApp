using Domain.DTO;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using System.Text.Json;

namespace Infrastructure.Services
{
    public sealed class BookingHistoryWriter :IBookingHistoryWriter
    {
        private readonly IBookingHistoryRepository _bookingHistoryRepository;

        public BookingHistoryWriter(IBookingHistoryRepository bookingHistoryRepository)
        {
            _bookingHistoryRepository = bookingHistoryRepository;
        }

        public async Task Add(Booking booking, ActionType action, IReadOnlyCollection<BookingFieldChange> changes, BookingActor actor)
        {
            var history = new BookingHistory
            {
                Booking = booking,
                ActionType = action,
                ChangedByUserId = actor.UserId ?? null,
                Source = actor.Source,

                ChangesJson = JsonSerializer.Serialize(changes),

            };

            await _bookingHistoryRepository.AddWithoutSave(history);
        }
    }
}
