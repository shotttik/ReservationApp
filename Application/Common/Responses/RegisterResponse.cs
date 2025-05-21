namespace Application.Common.Responses
{
    public class RegisterResponse
    {
        public string? URL { get; set; }
        public required string VerificationToken { get; set; }
        public DateTime VerificationTokenExpTime { get; set; }
    }
}
