namespace Application.Responses
{
    public class RegisterResponse
    {
        public string? Description { get; set; }
        public string? URL { get; set; }
        public required string VerificationToken { get; set; }
        public DateTime VerificationTokenExpTime { get; set; }
    }
}
