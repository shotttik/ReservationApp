using Domain.Entities.BranchReleated;
using Domain.Entities.CompanyReleated;
using Domain.Entities.ReviewReleated;
using Domain.Entities.User;
using Domain.Enums;
using System.Security.Cryptography;
namespace Domain.Entities.Common
{
    public class Booking :BaseEntity
    {
        public Booking()
        {
            Reference = GenerateReference();
        }
        public int? ClientID { get; set; }
        public UserAccount? Client { get; set; } = null!;
        public int EmployeeID { get; set; }
        public UserAccount Employee { get; set; } = null!;
        public int ServiceID { get; set; }
        public Service Service { get; set; } = null!;
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTimeExpected { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal PriceExpected { get; set; }
        public decimal PriceFull { get; set; }
        public decimal? Discount { get; set; }
        public decimal? PriceFinal { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public string? CancellationReason { get; set; }
        public string? Note { get; set; }
        public string Reference { get; private set; } = null!;
        public string? PromoCodeValue { get; set; } // snapshot
        public int? PromoCodeId { get; set; }
        public virtual PromoCode? PromoCode { get; set; }
        public virtual ReviewInvite ReviewInvite { get; set; } = null!;
        public virtual BookingGuestInfo? GuestInfo { get; set; }
        public virtual ICollection<BookingVerification> Verifications { get; set; } = [];
        public bool IsCompleted => Status == BookingStatus.Completed;

        public static string GenerateReference()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var bytes = RandomNumberGenerator.GetBytes(8);

            return "BK-" + string.Concat(bytes.Select(b => chars [b % chars.Length]));
        }
        public bool IsCancelable => Status == BookingStatus.Pending || Status == BookingStatus.Accepted;
        public void Cancel(string? cancelationReason)
        {
            CancellationReason = cancelationReason ?? null;
            Status = BookingStatus.Canceled;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateEndTimeExpected() => EndTimeExpected = StartTime.AddMinutes(Service.Duration);
        public bool IsReschedulable =>
            (Status == BookingStatus.Pending || Status == BookingStatus.Accepted)
            && StartTime > DateTime.UtcNow.AddMinutes(30);
    }
}
