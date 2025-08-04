using Application.Common.Requests.Booking;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;

namespace Application.Extensions.Mappers
{
    public static class BookingMapper
    {
        public static BookingDTO MapToDTO(this Domain.Entities.Common.Booking booking)
        {
            return new BookingDTO
            {
                ID = booking.ID,
                ClientID = booking.ClientID,
                EmployeeID = booking.EmployeeID,
                CompanyID = booking.CompanyID,
                ServiceName = booking.ServiceName,
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
                CreatedAt = booking.CreatedAt,
                UpdatedAt = booking.UpdatedAt
            };
        }

        public static Booking MapToEntity(
            this ClientBookingCreateRequest request,
            Service service,
            int? clientID,
            int companyID,
            int employeeID)
        {
            return new Booking
            {
                ClientID = clientID,
                CompanyID = companyID,
                EmployeeID = employeeID,
                ServiceName = service.Name,
                StartTime = request.StartTime,
                EndTimeExpected = request.StartTime.AddMinutes(service.Duration),
                PriceExpected = service.Price,
                Note = request.Note
            };
        }
    }
}
