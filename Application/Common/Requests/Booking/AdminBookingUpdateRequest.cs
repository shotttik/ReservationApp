using Domain.Enums;

namespace Application.Common.Requests.Booking
{
    public class AdminBookingUpdateRequest
    {
        public int? ClientID { get; set; }
        public int EmployeeID { get; set; }
        public int CompanyID { get; set; }
        public string ServiceName { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTimeExpected { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal PriceExpected { get; set; }
        public decimal? PriceFull { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PriceFinal { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? CancellationReason { get; set; }
        public string? Note { get; set; }
    }
}
