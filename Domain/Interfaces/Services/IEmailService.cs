namespace Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmail(string toEmail, string subject, string body);
    }
}
