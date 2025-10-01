using Application.Common.Requests.Booking;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<BookingDTO>> CreateByClient(ClientBookingCreateRequest request);
        Task<Result<BookingDTO>> CreateByAdmin(AdminBookingCreateRequest request);
        Task<Result<List<BookingDTO>>> GetWeeklyPublicData(int companyId, DateOnly targetDate);
        Task<Result> ChangeStatus(int bookingId, BookingStatusChangeRequest request);
        Task<Result<PagedList<BookingDTO>>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken);
    }
}
