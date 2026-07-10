using Domain.DTO.Branch;
using Domain.DTO.Company;
using Domain.DTO.User;
using Domain.Enums;

namespace Domain.DTO
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public int ServiceId { get; set; }
        public int CompanyId { get; set; }
        public string ServiceName { get; set; } = null!; // frontshi service rom ar wamoighos xolme saxelis gamo
        public DateTime StartTime { get; set; }
        public DateTime EndTimeExpected { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal PriceExpected { get; set; }
        public decimal? PriceFull { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PriceFinal { get; set; }
        public string? PromoCodeValue { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? CancellationReason { get; set; }
        public string? Note { get; set; }
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
