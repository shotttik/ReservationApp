using Application.Common.Requests.Booking;
using Application.Common.Results;
using Application.Interfaces;
using Application.Options;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.RabbitMq;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Utilities;

namespace Application.Services
{
    public class BookingVerificationService :IBookingVerificationService
    {
        private readonly BookingSettings _bookingSettings;
        private readonly ISmsTemplateBuilder _smsBuilder;
        private readonly IEmailTemplateBuilder _emailBuilder;
        private readonly IMessageProducerService _messageProducer;
        private readonly IBookingRepository _bookingRepository;
        private readonly IAccessGuard _accessGuard;
        private readonly IBookingVerificationRepository _bookingVerificationRepository;

        public BookingVerificationService(
            IOptions<BookingSettings> bookingSettings,
            IEmailTemplateBuilder emailBuilder,
            IMessageProducerService messageProducer,
            ISmsTemplateBuilder smsBuilder,
            IBookingRepository bookingRepository,
            IAccessGuard accessGuard,
            IBookingVerificationRepository bookingVerificationRepository)
        {
            _bookingSettings = bookingSettings.Value;
            _emailBuilder = emailBuilder;
            _messageProducer = messageProducer;
            _smsBuilder = smsBuilder;
            _bookingRepository = bookingRepository;
            _accessGuard = accessGuard;
            _bookingVerificationRepository = bookingVerificationRepository;
        }
        public (BookingVerification, string code) CreateBookingVerification(VerificationType verificationType)
        {
            var verification = new BookingVerification();
            var code = string.Empty;
            var codeHash = CodeHasher.GenerateAndHash(_bookingSettings.VerificationCodeLength, out code);

            verification.CodeHash = codeHash;
            verification.ExpiresAt = DateTime.Now.AddMinutes(_bookingSettings.VerificationCodeExpirationMinutes);
            verification.VerificationType = verificationType;

            return (verification, code);
        }
        public async Task SendVerificationNotification(
            VerificationType verificationType,
            string contact,
            string? displayName,
            string code
            )
        {
            switch (verificationType)
            {
                case VerificationType.Email:
                    var emailMessage = _emailBuilder.BuildCodeVerification(
                        contact,
                        displayName,
                        code,
                        _bookingSettings.VerificationCodeExpirationMinutes);

                    await _messageProducer.PublishEmailAsync(emailMessage);
                    break;
                case VerificationType.Phone:
                    var smsMessage = _smsBuilder.BuildCodeVerification(
                        contact,
                        code,
                        _bookingSettings.VerificationCodeExpirationMinutes);
                    await _messageProducer.PublishSmsAsync(smsMessage);
                    break;
            }
        }
        public async Task<Result> Verify(int bookingId, BookingVerificationRequest request)
        {
            var booking = await _bookingRepository.GetWithVerificationsAndGuestInfo(bookingId);
            if (booking == null || booking.GuestInfo == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            if (booking.Status != BookingStatus.PendingVerification)
            {
                return Result.Failure(BookingResults.DoesntRequireVerification);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.ID, booking.ClientID, booking.EmployeeID, booking.CompanyID);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }

            var bookingVerification = booking.Verifications.
                OrderByDescending(e => e.CreatedAt).
                Where(e => e.ExpiresAt > DateTime.Now && e.VerifiedAt == null).
                FirstOrDefault();
            if (bookingVerification == null)
            {
                return Result.Failure(BookingResults.VerificationCodeExpired);
            }
            var valid = CodeHasher.CompareCodeAndHash(request.Code, bookingVerification.CodeHash);
            if (!valid)
            {
                return Result.Failure(BookingResults.VerificationCodeIsWrong);
            }

            bookingVerification.Verify();
            booking.Status = BookingStatus.Pending;

            await _bookingRepository.Update(booking);

            return Result.Success();
        }
        public async Task<Result> ResendCode(int bookingId)
        {
            var booking = await _bookingRepository.GetWithVerificationsAndGuestInfo(bookingId);
            if (booking == null || booking.GuestInfo == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            if (booking.Status != BookingStatus.PendingVerification)
                return Result.Failure(BookingResults.DoesntRequireVerification);

            var error = await _accessGuard.EnsureAccessToBooking(booking.ID, booking.ClientID, booking.EmployeeID, booking.CompanyID);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }

            var bookingVerifications = booking.Verifications.Where(e => e.ExpiresAt > DateTime.Now);
            if (!bookingVerifications.IsNullOrEmpty())
            {
                return Result.Failure(BookingResults.WaitingForVerification);
            }

            string code = string.Empty;

            (var bookingVerification, code) = CreateBookingVerification(booking.GuestInfo.ContactType);
            bookingVerification.BookingId = bookingId;
            await _bookingVerificationRepository.Add(bookingVerification);

            await SendVerificationNotification(
                booking.GuestInfo.ContactType,
                booking.GuestInfo.Contact,
                booking.GuestInfo.DisplayName,
                code);

            return Result.Success();
        }
    }
}
