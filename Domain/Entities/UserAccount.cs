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
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
       
        public UserLoginData UserLoginData { get; set; } = null!;
        public ICollection<Role> Roles { get; set; } = [];
        public Company? Company { get; set; }
    }
}
