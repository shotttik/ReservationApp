using Application.Interfaces;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class BookingService :IBookingService
    {
        private readonly IBookingRepository bookingRepository;

        public BookingService(
            IBookingRepository bookingRepository)
        {
            this.bookingRepository = bookingRepository;
        }

        public void Create()
        {
            // Logic to create a booking
        }
    }
}
