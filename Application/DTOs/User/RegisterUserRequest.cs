namespace Application.DTOs.User
{
    public class RegisterUserRequest
    {

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}