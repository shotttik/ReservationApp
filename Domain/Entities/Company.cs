namespace Domain.Entities
{
    public class Company
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string IN { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<UserAccount> UserAccounts { get; set; } = [];
    }
}
