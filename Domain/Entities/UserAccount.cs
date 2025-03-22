namespace Domain.Entities
{
    public class UserAccount
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? CompanyID { get; set; }
        public required int RoleID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual UserLoginData? UserLoginData { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Company? Company { get; set; }

        public void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;

    }
}
