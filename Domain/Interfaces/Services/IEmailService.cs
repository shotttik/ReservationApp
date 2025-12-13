using Shared.RabbitMq;

namespace Domain.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailMessage message);
    }
}
