namespace Domain.Entities
{
    public class CompanyInvitation
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int UserAccountID { get; set; }
        public string? Token { get; set; } = null!;
        public DateTime? ExpirationTime { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Company Company { get; set; } = null!;
        public void UpdateTimestamp() => UpdatedAt = DateTime.Now;
    }
}
