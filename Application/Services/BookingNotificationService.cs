using Application.Interfaces;
using Application.Options;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class BookingNotificationService :IBookingNotificationService
    {
        private readonly BookingOptions _bookingSettings;
        private readonly IEmailTemplateBuilder _emailBuilder;
        private readonly ISmsTemplateBuilder _smsBuilder;
        private readonly IMessageProducerService _messageProducer;
        private readonly ILogger<BookingNotificationService> _logger;

        public BookingNotificationService(
            IOptions<BookingOptions> bookingSettings,
            IEmailTemplateBuilder emailBuilder,
            ISmsTemplateBuilder smsBuilder,
            IMessageProducerService messageProducer,
            ILogger<BookingNotificationService> logger)
        {
            _bookingSettings = bookingSettings.Value;
            _emailBuilder = emailBuilder;
            _smsBuilder = smsBuilder;
            _messageProducer = messageProducer;
            _logger = logger;
        }

        public async Task SendVerificationCodeAsync(
            VerificationType verificationType,
            string contact,
            string? displayName,
            string code,
            Booking booking,
            CancellationToken cancellationToken = default)
        {
            try
            {
                switch (verificationType)
                {
                    case VerificationType.Email:
                        var emailMessage = _emailBuilder.BuildCodeVerification(
                            contact,
                            displayName,
                            code,
                            _bookingSettings.VerificationCodeExpirationMinutes,
                            booking);

                        await _messageProducer.PublishEmailAsync(emailMessage, cancellationToken);
                        break;

                    case VerificationType.Phone:
                        var smsMessage = _smsBuilder.BuildCodeVerification(
                            contact,
                            code,
                            _bookingSettings.VerificationCodeExpirationMinutes);

                        await _messageProducer.PublishSmsAsync(smsMessage, cancellationToken);
                        break;

                    default:
                        _logger.LogWarning(
                            "Unsupported booking verification notification type {VerificationType} for booking {BookingReference}.",
                            verificationType,
                            booking.Reference);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to publish booking verification notification for booking {BookingReference} via {VerificationType}.",
                    booking.Reference,
                    verificationType);
            }
        }
    }
}
