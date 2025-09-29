using Domain.Entities.CompanyReleated;
using Domain.Entities.User;
using Domain.Enums;
namespace Domain.Entities.Common
{
    public class Booking :BaseEntity
    {
        public int? ClientID { get; set; }
        public UserAccount? Client { get; set; } = null!;
        public int EmployeeID { get; set; }
        public UserAccount Employee { get; set; } = null!;
        public int CompanyID { get; set; }
        public Company Company { get; set; } = null!;
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

        public bool IsCompleted => Status == BookingStatus.Completed;
    }
}
