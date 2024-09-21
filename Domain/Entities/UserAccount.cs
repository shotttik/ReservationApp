namespace Domain.Entities
{
    public class UserAccount
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string IdentificationNumber { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required int RoleID { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public UserRole Role { get; set; } = null!;
        public UserLoginData UserLoginData { get; set; } = null!;
    }
}
