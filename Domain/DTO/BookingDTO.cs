using Domain.Enums;

namespace Domain.DTO
{
    public class BookingDTO
    {
        public int ID { get; set; }
        public int? ClientID { get; set; }
        public int EmployeeID { get; set; }
        public int BranchId { get; set; }
        public int ServiceID { get; set; } 
        public string ServiceName { get; set; } = null!; // frontshi service rom ar wamoighos xolme saxelis gamo
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
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
