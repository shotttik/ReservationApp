namespace Application.Common.Responses
{
    public class CreateGuestTokenResponse
    {
        public string Token { get; set; } = null!;
        public int ExpiresInMinutes { get; set; }
    }
}
