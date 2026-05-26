using Application.Authentication;
using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Interfaces;
using Application.Options;
using Domain.Entities.Common;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Options;
using Shared.Utilities;

namespace Application.Services
{
    public class GuestBookingService :IGuestBookingService
    {
        private readonly BookingSettings _bookingSettings;
        private readonly ISmsTemplateBuilder _smsBuilder;
        private readonly IEmailTemplateBuilder _emailBuilder;
        private readonly IMessageProducerService _messageProducer;
        private readonly IBookingRepository _bookingRepository;
        private readonly IAccessGuard _accessGuard;
        private readonly IBookingVerificationRepository _bookingVerificationRepository;

        public GuestBookingService(
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
            string code,
            string bookingReference
            )
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
                            bookingReference);

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
            catch (Exception)
            {
                //TODO can be done something with it pass
            }
        }
        public async Task<Result> Verify(int bookingId, BookingVerificationRequest request)
        {
            var data = await _bookingRepository.GetWithGuestInfoAndLatestPendingVerification(bookingId);

            if (data == null || data.LatestPendingVerification == null)
                return Result.Failure(BookingResults.NotValidForVerification);

            var booking = data.Booking;
            var bookingVerification = data.LatestPendingVerification;

            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }

            if (bookingVerification.ExpiresAt < DateTime.Now)
            {
                return Result.Failure(BookingResults.VerificationCodeExpired);
            }
            var valid = CodeHasher.CompareCodeAndHash(request.Code, bookingVerification.CodeHash);
            if (!valid)
            {
                return Result.Failure(BookingResults.VerificationCodeIsWrong);
            }

            if (bookingVerification.PendingNewContact != null) // roca axali contactis change request aris gamogzavnili
            {
                booking.GuestInfo!.Contact = bookingVerification.PendingNewContact;
                booking.GuestInfo.ContactType = bookingVerification.VerificationType;
            }
            bookingVerification.Verify();
            booking.Status = BookingStatus.Pending;

            await _bookingRepository.Update(booking);

            return Result.Success(BookingResults.VerifiedSuccess);
        }
        public async Task<Result> ResendVerificationCode(int bookingId)
        {
            var data = await _bookingRepository.GetWithGuestInfoAndLatestPendingVerification(bookingId);

            if (data == null || data.LatestPendingVerification == null)
                return Result.Failure(BookingResults.NotValidForVerification);
            var booking = data.Booking;
            var bookingVerification = data.LatestPendingVerification;

            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }

            if (bookingVerification.ExpiresAt > DateTime.Now)
            {
                return Result.Failure(BookingResults.WaitingForVerification);
            }

            var (newBookingVerification, code) = CreateBookingVerification(booking.GuestInfo!.ContactType);
            newBookingVerification.BookingId = bookingId;
            await _bookingVerificationRepository.Add(newBookingVerification);

            await SendVerificationNotification(
                booking.GuestInfo.ContactType,
                booking.GuestInfo.Contact,
                booking.GuestInfo.DisplayName,
                code,
                booking.Reference);

            return Result.Success(BookingResults.VerificationCodeSent);
        }
        public async Task<Result> SendGuestAccessCode(GuestBookingAccessRequest request)
        {
            var (reference, contact) = request;
            var data = await _bookingRepository.GetWithGuestInfoAndLatestPendingVerification(reference, contact);
            if (data == null)
            {
                return Result.Failure(BookingResults.NotValidForGuestAccess);
            }
            var booking = data.Booking;

            if (data.LatestPendingVerification?.ExpiresAt > DateTime.Now)
            {
                return Result.Failure(BookingResults.WaitingForVerification);
            }

            var (newBookingVerification, code) = CreateBookingVerification(booking.GuestInfo!.ContactType);
            newBookingVerification.BookingId = booking.Id;
            await _bookingVerificationRepository.Add(newBookingVerification);

            await SendVerificationNotification(
                booking.GuestInfo.ContactType,
                booking.GuestInfo.Contact,
                booking.GuestInfo.DisplayName,
                code,
                booking.Reference);

            return Result.Success(BookingResults.VerificationCodeSent);
        }

        public async Task<Result<CreateGuestTokenResponse>> VerifyGuestAccess(GuestBookingAccessVerifyRequest request)
        {
            var data = await _bookingRepository.GetWithGuestInfoAndLatestPendingVerification(request.Reference);

            if (data == null || data.LatestPendingVerification == null)
                return Result.Failure<CreateGuestTokenResponse>(BookingResults.NotValidForVerification);

            var booking = data.Booking;
            var bookingVerification = data.LatestPendingVerification;
            if (bookingVerification.ExpiresAt < DateTime.Now)
            {
                return Result.Failure<CreateGuestTokenResponse>(BookingResults.VerificationCodeExpired);
            }
            var valid = CodeHasher.CompareCodeAndHash(request.Code, bookingVerification.CodeHash);
            if (!valid)
            {
                return Result.Failure<CreateGuestTokenResponse>(BookingResults.VerificationCodeIsWrong);
            }

            bookingVerification.Verify();
            booking.Status = BookingStatus.Pending;

            await _bookingRepository.Update(booking);

            var token = JWTGenerator.GenerateGuestToken(booking.Id, _bookingSettings);
            var response = new CreateGuestTokenResponse()
            {
                Token = token,
                ExpiresInMinutes = _bookingSettings.GuestToken.ExpirationMinutes
            };

            return Result.Success(response);
        }
        public async Task<Result> UpdateGuestInfoContact(int routeBookingId, BookingGuestInfoContactUpdateRequest request)
        {
            var data = await _bookingRepository.GetContactUpdatableWithLatestPendingVerification(routeBookingId);
            if (data == null)
                return Result.Failure(BookingResults.NotValidForVerification);

            var booking = data.Booking;

            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }
            if (data.LatestPendingVerification != null && data.LatestPendingVerification!.ExpiresAt > DateTime.Now)
            {
                return Result.Failure(BookingResults.WaitingForVerification);
            }

            var (newBookingVerification, code) = CreateBookingVerification(request.ContactType);
            newBookingVerification.BookingId = routeBookingId;
            newBookingVerification.VerificationType = request.ContactType;
            newBookingVerification.PendingNewContact = request.PendingNewContact;
            booking.Status = BookingStatus.PendingVerification;
            booking.Verifications.Add(newBookingVerification);
            await _bookingRepository.Update(booking);
            await SendVerificationNotification(
                request.ContactType,
                request.PendingNewContact,
                booking.GuestInfo!.DisplayName,
                code,
                booking.Reference);

            return Result.Success(BookingResults.VerificationCodeSent);
        }
    }
}
