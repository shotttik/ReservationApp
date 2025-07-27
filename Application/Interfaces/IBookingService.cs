using Application.Common.Requests.Booking;
using Application.Common.Results;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<BookingDTO>> Create(BookingCreateRequest request);
    }
}
