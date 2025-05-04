namespace Application.Responses
{
    public class RegisterResponse
    {
        public string? Description { get; set; } = null;
        public string? URL { get; set; }
        public required string VerificationToken { get; set; }
        public DateTime VerificationTokenExpTime { get; set; }

        public void SetDefaultDescription(double expDays)
        {
            Description = $"User registered successfully, Now You have to Verify your email, check inbox, you have {expDays} days.";
        }
    }
}
