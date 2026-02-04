using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Domain.Abstractions;
using Domain.DTO;

namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result<CreateBookingByGuestResponse>> CreateByGuest(GuestBookingCreateRequest request);
        Task<Result<BookingDTO>> CreateByClient(ClientBookingCreateRequest request);
        Task<Result<BookingDTO>> CreateByAdmin(AdminBookingCreateRequest request);
        Task<Result<List<BookingDTO>>> GetWeeklyPublicData(int companyId, DateOnly targetDate);
        Task<Result> ChangeStatus(int bookingId, BookingStatusChangeRequest request);
        Task<Result<PagedList<BookingDTO>>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken);
        Task<Result> Delete(int bookingId);
        Task<Result> CancelBooking(int bookingId, BookingCancelRequest? request);
        Task<Result<BookingDTO>> RescheduleBooking(int bookingId, RescheduleBookingRequest request);
    }
}
