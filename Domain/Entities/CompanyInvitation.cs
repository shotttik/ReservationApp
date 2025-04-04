namespace Domain.Entities
{
    public class CompanyInvitation
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int MemberID { get; set; }
        public string Token { get; set; } = null!;
        public DateTime ExpirationTime { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Company Company { get; set; } = null!;
    }
}
