using Shared.RabbitMq;

namespace Domain.Interfaces.Services
{
    public interface IEmailTemplateBuilder
    {
        public EmailMessage BuildVerification(string toEmail, string firstName, string verificationLink);
        public EmailMessage BuildCodeVerification(string toEmail, string? firstName, string verificationCode, int expMinutes, string bookingReference);
    }
}
