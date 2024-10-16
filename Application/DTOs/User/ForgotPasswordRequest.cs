namespace Application.DTOs.User
{
    public record ForgotPasswordRequest
    {
        public required string Email { get; set; }

    }
}
