using Domain.Entities.Common;

namespace Domain.Entities.CompanyReleated
{
    public class CompanyInvitation :BaseEntity
    {
        public int CompanyID { get; set; }
        public int UserAccountID { get; set; }
        public string? Token { get; set; } = null!;
        public DateTime? ExpirationTime { get; set; }
        public bool IsAccepted { get; set; }
        public int BranchId { get; set; }
        public Company Company { get; set; } = null!;
    }
}
