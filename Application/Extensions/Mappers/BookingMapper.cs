using Application.Common.Requests.Booking;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;

namespace Application.Extensions.Mappers
{
    public static class BookingMapper
    {
        public static BookingDTO MapToDTO(this Domain.Entities.Common.Booking booking, bool showRef = false)
        {
            return new BookingDTO
            {
                Id = booking.ID,
                ClientId = booking.ClientID,
                EmployeeId = booking.EmployeeID,
                BranchId = booking.BranchId,
                ServiceId = booking.Service.ID,
                ServiceName = booking.Service.Name,
                StartTime = booking.StartTime,
                EndTimeExpected = booking.EndTimeExpected,
                EndTime = booking.EndTime,
                PriceExpected = booking.PriceExpected,
                PriceFull = booking.PriceFull,
                Discount = booking.Discount,
                PriceFinal = booking.PriceFinal,
                Status = booking.Status,
                CancellationReason = booking.CancellationReason,
                Note = booking.Note,
                Reference = showRef ? booking.Reference : null,
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt
            };
        }

        public static Booking MapToEntity(
            this ClientBookingCreateRequest request,
            Service service,
            int? clientID,
            int branchId,
            int employeeID)
        {
            return new Booking
            {
                ClientID = clientID,
                BranchId = branchId,
                EmployeeID = employeeID,
                Service = service,
                StartTime = request.StartTime,
                EndTimeExpected = request.StartTime.AddMinutes(service.Duration),
                PriceExpected = service.Price,
                Note = request.Note
            };
        }
    }
}
