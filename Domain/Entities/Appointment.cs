using Domain.Enums;

namespace Domain.Entities
{
    public class Appointment
    {
        public int ID { get; set; }
        public int ClientID { get; set; }
        public UserAccount Client { get; set; } = null!;
        public int EmployeeID { get; set; }
        public UserAccount Employee { get; set; } = null!;
        public int? CompanyID { get; set; }
        public Company? Company { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTimeExpected { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal PriceExpected { get; set; }
        public decimal? PriceFull { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PriceFinal { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string? CancellationReason { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public void UpdateTimestamp() => UpdatedAt = DateTime.Now;
    }
}
