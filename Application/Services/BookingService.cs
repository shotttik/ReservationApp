using Application.Authentication;
using Application.Common.Notifications;
using Application.Common.Requests.Booking;
using Application.Common.Responses;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Application.Options;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Entities.User;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class BookingService :IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IAuthService _authService;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IAccessGuard _accessGuard;
        private readonly IGuestBookingService _guestBookingService;
        private readonly IBookingNotificationService _bookingNotificationService;
        private readonly IRealtimeNotificationService _realtimeNotificationService;
        private readonly BookingOptions _bookingSettings;
        private readonly ISubscriptionGuard _subscriptionGuard;
        private readonly IPromoService _promoService;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookingHistoryWriter _bookingHistoryWriter;
        private readonly IBookingHistoryRepository _bookingHistoryRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserAccountRepository userAccountRepository,
            IAccessGuard accessGuard,
            IGuestBookingService guestBookingService,
            IBookingNotificationService bookingNotificationService,
            IRealtimeNotificationService realtimeNotificationService,
            IOptions<BookingOptions> bookingSettings,
            ISubscriptionGuard subscriptionGuard,
            IPromoService promoService,
            IPromoCodeRepository promoCodeRepository,
            IUnitOfWork unitOfWork,
            IBookingHistoryWriter bookingHistoryWriter,
            IBookingHistoryRepository bookingHistoryRepository
            )
        {
            _bookingRepository = bookingRepository;
            _authService = authService;
            _userAccountRepository = userAccountRepository;
            _accessGuard = accessGuard;
            _guestBookingService = guestBookingService;
            _bookingNotificationService = bookingNotificationService;
            _realtimeNotificationService = realtimeNotificationService;
            _bookingSettings = bookingSettings.Value;
            _subscriptionGuard = subscriptionGuard;
            _promoService = promoService;
            _promoCodeRepository = promoCodeRepository;
            _unitOfWork = unitOfWork;
            _bookingHistoryWriter = bookingHistoryWriter;
            _bookingHistoryRepository = bookingHistoryRepository;
        }
        public async Task<Result<CreateBookingByGuestResponse>> CreateByGuest(GuestBookingCreateRequest request)
        {
            var employeeResult = await GetValidEmployee(request.EmployeeId);
            if (!employeeResult.IsSuccess)
                return Result.Failure<CreateBookingByGuestResponse>(employeeResult.Error);
            var employee = employeeResult.Value!;

            var service = employee!.Company!.Services.FirstOrDefault(s => s.Id == request.ServiceId);
            if (service == null)
            {
                return Result.Failure<CreateBookingByGuestResponse>(BookingResults.ServiceDoesntExists);
            }
            var companyId = (int)employee.CompanyID!;
            var subscriptionError = await _subscriptionGuard.EnsureCanCreateBookingAsync(companyId);
            if (subscriptionError != Error.None)
            {
                return Result.Failure<CreateBookingByGuestResponse>(subscriptionError);
            }
            var requestValidationError = await ValidateCreateRequest(service, employee, request.StartTime, null);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<CreateBookingByGuestResponse>(requestValidationError);
            }
            var booking = request.MapToEntity(service, null, (int)employee.BranchId!, employee.Id);
            var bookingGuestInfo = new BookingGuestInfo()
            {
                ContactType = request.GuestInfo.ContactType,
                Contact = request.GuestInfo.Contact,
                DisplayName = request.GuestInfo.DisplayName
            };

            decimal finalPrice = booking.PriceExpected;
            PromoCode? appliedPromo = null;

            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promoResult = await _promoService.ApplyPromo(request.PromoCode, companyId, service.Id);

                if (!promoResult.IsValid)
                    return Result.Failure<CreateBookingByGuestResponse>(promoResult.Error);

                finalPrice -= promoResult.Discount;

                if (finalPrice < 0)
                    finalPrice = 0;

                booking.PriceFinal = finalPrice;

                appliedPromo = promoResult.Promo;

                booking.PromoCodeValue = appliedPromo!.Code;
                booking.Discount = promoResult.Discount;
                booking.PromoCodeId = appliedPromo.Id;
            }
            (var bookingVerification, var code) = _guestBookingService.CreateBookingVerification(request.GuestInfo.ContactType);
            booking.Status = BookingStatus.PendingVerification;
            booking.GuestInfo = bookingGuestInfo;
            booking.Verifications.Add(bookingVerification);

            await _bookingRepository.Add(booking);
            if (appliedPromo != null)
            {
                appliedPromo.UsedCount++;
                await _promoCodeRepository.Update(appliedPromo);
            }
            await _bookingNotificationService.SendVerificationCodeAsync(
                bookingGuestInfo.ContactType,
                bookingGuestInfo.Contact,
                bookingGuestInfo.DisplayName,
                code,
                booking);

            var token = JWTGenerator.GenerateGuestToken(booking.Id, _bookingSettings);
            var bookingDTO = booking.MapToDTO();
            await NotifyBookingAsync(
                booking,
                BookingNotificationResults.BookingCreatedByGuest(service.Name),
                bookingDTO);

            var response = new CreateBookingByGuestResponse()
            {
                Booking = bookingDTO,
                GuestToken = new CreateGuestTokenResponse()
                {
                    Token = token,
                    ExpiresInMinutes = _bookingSettings.GuestToken.ExpirationMinutes
                }
            };

            return response;
        }
        public async Task<Result<BookingDTO>> CreateByClient(ClientBookingCreateRequest request)
        {
            var authUser = await _authService.GetCurrentUser();

            var employeeResult = await GetValidEmployee(request.EmployeeId);
            if (!employeeResult.IsSuccess)
                return Result.Failure<BookingDTO>(employeeResult.Error);
            var employee = employeeResult.Value!;

            var service = employee!.Company!.Services.FirstOrDefault(s => s.Id == request.ServiceId);
            if (service == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.ServiceDoesntExists);
            }

            var companyId = (int)employee.CompanyID!;
            var subscriptionError = await _subscriptionGuard.EnsureCanCreateBookingAsync(companyId);
            if (subscriptionError != Error.None)
            {
                return Result.Failure<BookingDTO>(subscriptionError);
            }
            var requestValidationError = await ValidateCreateRequest(service, employee, request.StartTime, authUser.UserAccountId);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }

            var booking = request.MapToEntity(service, authUser.UserAccountId, (int)employee.BranchId!, employee.Id);
            decimal finalPrice = booking.PriceExpected;
            PromoCode? appliedPromo = null;

            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promoResult = await _promoService.ApplyPromo(request.PromoCode, companyId, service.Id);

                if (!promoResult.IsValid)
                    return Result.Failure<BookingDTO>(promoResult.Error);

                finalPrice -= promoResult.Discount;

                if (finalPrice < 0)
                    finalPrice = 0;

                booking.PriceFinal = finalPrice;

                appliedPromo = promoResult.Promo;

                booking.PromoCodeValue = appliedPromo!.Code;
                booking.Discount = promoResult.Discount;
                booking.PromoCodeId = appliedPromo.Id;
            }
            await _bookingRepository.Add(booking);
            if (appliedPromo != null)
            {
                appliedPromo.UsedCount++;
                await _promoCodeRepository.Update(appliedPromo);
            }
            var bookingDTO = booking.MapToDTO(showRef: true);
            await NotifyBookingAsync(
                booking,
                BookingNotificationResults.BookingCreatedByClient(service.Name),
                bookingDTO);

            return Result.Success(bookingDTO);
        }

        public async Task<Result<BookingDTO>> CreateByAdmin(AdminBookingCreateRequest request)
        {
            BookingDTO bookingDTO;
            var employeeResult = await GetValidEmployee(request.EmployeeId);
            if (!employeeResult.IsSuccess)
                return Result.Failure<BookingDTO>(employeeResult.Error);
            var employee = employeeResult.Value!;
            var companyID = employee.CompanyID!.Value;

            var accessError = await _accessGuard.EnsureAccessToCompanyEmployee(companyID, employee.UserLoginDataID);
            if (accessError != Error.None)
            {
                return Result.Failure<BookingDTO>(accessError);
            }
            var service = employee.Company!.Services.FirstOrDefault(s => s.Id == request.ServiceId);
            if (service == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.ServiceDoesntExists);
            }

            var companyId = (int)employee.CompanyID!;
            var subscriptionError = await _subscriptionGuard.EnsureCanCreateBookingAsync(companyId);
            if (subscriptionError != Error.None)
            {
                return Result.Failure<BookingDTO>(subscriptionError);
            }

            BookingGuestInfo? bookingGuestInfo = null;
            BookingVerification? bookingVerification = null;
            string code = string.Empty;
            if (request.ClientId != null)
            {
                var clientAccount = await _userAccountRepository.GetByUserLoginDataIDWithBookingData((int)request.ClientId);
                if (clientAccount == null)
                {
                    return Result.Failure<BookingDTO>(BookingResults.ClientDoesntExists);
                }
            }
            else if (request.GuestInfo != null)
            {
                bookingGuestInfo = new BookingGuestInfo()
                {
                    ContactType = request.GuestInfo.ContactType,
                    Contact = request.GuestInfo.Contact,
                    DisplayName = request.GuestInfo.DisplayName
                };

                (bookingVerification, code) = _guestBookingService.CreateBookingVerification(request.GuestInfo.ContactType);
            }
            else
            {
                return Result.Failure<BookingDTO>(BookingResults.ClientOrGuestInfoMustBeProvided);
            }
            var requestValidationError = await ValidateCreateRequest(service, employee, request.StartTime, request.ClientId);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }

            var booking = request.MapToEntity(service, request.ClientId, (int)employee.BranchId!, employee.Id);
            decimal finalPrice = booking.PriceExpected;
            PromoCode? appliedPromo = null;

            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promoResult = await _promoService.ApplyPromo(request.PromoCode, companyId, service.Id);

                if (!promoResult.IsValid)
                {
                    return Result.Failure<BookingDTO>(promoResult.Error);
                }

                finalPrice -= promoResult.Discount;

                if (finalPrice < 0)
                    finalPrice = 0;

                booking.PriceFinal = finalPrice;

                appliedPromo = promoResult.Promo;

                booking.PromoCodeValue = appliedPromo!.Code;
                booking.Discount = promoResult.Discount;
                booking.PromoCodeId = appliedPromo.Id;
            }
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                booking.Status = BookingStatus.Accepted;
                if (bookingGuestInfo != null && bookingVerification != null)
                {
                    booking.Status = BookingStatus.PendingVerification;
                    booking.GuestInfo = bookingGuestInfo;
                    booking.Verifications.Add(bookingVerification);
                }
                await _bookingRepository.AddWithoutSave(booking);

                if (appliedPromo != null)
                {
                    appliedPromo.UsedCount++;
                    await _promoCodeRepository.Update(appliedPromo);
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
                if (bookingGuestInfo != null)
                {
                    await _bookingNotificationService.SendVerificationCodeAsync(
                        bookingGuestInfo.ContactType,
                        bookingGuestInfo.Contact,
                        bookingGuestInfo.DisplayName,
                        code,
                        booking);
                }
                bookingDTO = booking.MapToDTO();
                await NotifyBookingAsync(
                    booking,
                    BookingNotificationResults.BookingCreatedByAdmin(service.Name),
                    bookingDTO);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            return Result.Success(bookingDTO);
        }
        public async Task<Result<List<BookingDTO>>> GetWeeklyPublicData(int branchId, DateOnly targetDate)
        {
            var endDate = targetDate.AddDays(7);
            var data = await _bookingRepository.GetDataForAllActiveEmployees(branchId, targetDate, endDate);
            var bookings = data.Select(e => e.MapToDTO()).ToList();

            return Result.Success(bookings);
        }
        public async Task<Result> ChangeStatus(int bookingId, BookingStatusChangeRequest request)
        {
            var booking = await _bookingRepository.GetWithBranch(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }
            if (booking.Status == request.Status)
            {
                return Result.Failure(BookingResults.SameStatus);
            }
            if (booking.Status == BookingStatus.Completed)
            {
                return Result.Failure(BookingResults.CannotChangeStatus);
            }
            var changes = new List<BookingFieldChange>();

            var oldStatus = booking.Status;
            var oldEndTime = booking.EndTime;
            var oldPriceFinal = booking.PriceFinal;
            var oldCancellationReason = booking.CancellationReason;

            if (request.IsCompleted)
            {
                booking.EndTime = DateTime.UtcNow;
                booking.PriceFinal = booking.PriceExpected - (booking.Discount ?? 0m);
            }
            if (request.IsCanceled || request.IsFailed)
            {
                booking.CancellationReason = request.CancellationReason;
            }
            booking.Status = request.Status;
            AddChange(
                changes,
                nameof(Booking.Status),
                oldStatus,
                booking.Status);

            AddChange(
                changes,
                nameof(Booking.EndTime),
                oldEndTime,
                booking.EndTime);

            AddChange(
                changes,
                nameof(Booking.PriceFinal),
                oldPriceFinal,
                booking.PriceFinal);

            AddChange(
                changes,
                nameof(Booking.CancellationReason),
                oldCancellationReason,
                booking.CancellationReason);

            var actor = GetCurrentBookingActor(booking.Id);

            await _bookingHistoryWriter.Add(
                booking,
                ActionType.StatusChanged,
                changes,
                actor);

            await _bookingRepository.UpdateWithoutSave(booking);

            await NotifyBookingAsync(
                booking,
                BookingNotificationResults.BookingStatusChanged(booking.Reference, booking.Status.ToString()),
                booking.MapToDTO());
            return Result.Success(BookingResults.StatusChanged);
        }
        public async Task<Result<PagedList<BookingDTO>>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken)
        {
            var authUser = await _authService.GetCurrentUser();
            var allowedFields = BookingFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(Booking));

            if (errors.Any())
            {
                return Result.Failure<PagedList<BookingDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }

            var bookings = await _bookingRepository.RetrievePaged(
                parameters,
                authUser,
                cancellationToken);

            return bookings;
        }
        public async Task<Result> Delete(int bookingId)
        {
            var booking = await _bookingRepository.Get(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }

            await _bookingRepository.Delete(booking);

            return Result.Success(BookingResults.Deleted);
        }
        public async Task<Result> CancelBooking(int bookingId, BookingCancelRequest? request)
        {
            var booking = await _bookingRepository.GetWithBranch(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }
            if (!booking.IsCancelable)
            {
                return Result.Failure(BookingResults.IsNotCancelable);
            }
            var oldStatus = booking.Status;
            var oldCancellationReason =
                booking.CancellationReason;
            booking.Cancel(request?.CancellationReason);
            var changes = new List<BookingFieldChange>();

            AddChange(
                changes,
                nameof(Booking.Status),
                oldStatus,
                booking.Status);

            AddChange(
                changes,
                nameof(Booking.CancellationReason),
                oldCancellationReason,
                booking.CancellationReason);

            await _bookingHistoryWriter.Add(
                booking,
                ActionType.Canceled,
                changes,
                GetCurrentBookingActor(booking.Id));

            await _unitOfWork.SaveChangesAsync();

            await NotifyBookingAsync(
                booking,
                BookingNotificationResults.BookingCancelled(booking.Reference, booking.Service.Name),
                booking.MapToDTO());

            return Result.Success(BookingResults.Canceled);
        }
        public async Task<Result<BookingDTO>> RescheduleBooking(int bookingId, RescheduleBookingRequest request)
        {
            var booking = await _bookingRepository.GetWithBranch(bookingId);
            if (booking == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.NotFound);
            }
            var employeeResult = await GetValidEmployee(request.EmployeeId);
            if (!employeeResult.IsSuccess)
                return Result.Failure<BookingDTO>(employeeResult.Error);
            var employee = employeeResult.Value!;
            if (employee.CompanyID != booking.Branch.CompanyId)
            {
                return Result.Failure<BookingDTO>(BookingResults.EmployeeIsInDifferentCompany);
            }
            var service = employee!.Company!.Services.FirstOrDefault(s => s.Id == request.ServiceId);
            if (service == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.ServiceDoesntExists);
            }
            var requestValidationError = await ValidateCreateRequest(service, employee, request.StartTime, booking.ClientID, bookingId);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure<BookingDTO>(error);
            }

            if (!booking.IsReschedulable)
            {
                return Result.Failure<BookingDTO>(BookingResults.IsNotReschedulable);
            }
            var oldServiceId = booking.ServiceID;
            var oldEmployeeId = booking.EmployeeID;
            var oldStartTime = booking.StartTime;
            var oldEndTimeExpected = booking.EndTimeExpected;
            var oldStatus = booking.Status;

            booking.Service = service;
            booking.ServiceID = service.Id;
            booking.Employee = employee;
            booking.EmployeeID = employee.Id;
            booking.StartTime = request.StartTime;
            booking.UpdateEndTimeExpected();
            booking.Status = BookingStatus.Pending;
            booking.UpdateTimestamp();

            var changes = new List<BookingFieldChange>();

            AddChange(
                changes,
                nameof(Booking.ServiceID),
                oldServiceId,
                booking.ServiceID);

            AddChange(
                changes,
                nameof(Booking.EmployeeID),
                oldEmployeeId,
                booking.EmployeeID);

            AddChange(
                changes,
                nameof(Booking.StartTime),
                oldStartTime,
                booking.StartTime);

            AddChange(
                changes,
                nameof(Booking.EndTimeExpected),
                oldEndTimeExpected,
                booking.EndTimeExpected);

            AddChange(
                changes,
                nameof(Booking.Status),
                oldStatus,
                booking.Status);

            await _bookingHistoryWriter.Add(
                booking,
                ActionType.Rescheduled,
                changes,
                GetCurrentBookingActor(booking.Id));

            await _unitOfWork.SaveChangesAsync();

            var updatedBookingDTO = booking.MapToDTO();
            await NotifyBookingAsync(
                booking,
                BookingNotificationResults.BookingRescheduled(booking.Reference, request.StartTime),
                updatedBookingDTO);

            return updatedBookingDTO;
        }
        public async Task<Result<BookingFullDTO>> GetFullData(int bookingId)
        {

            var booking = await _bookingRepository.GetFullData(bookingId);
            if (booking == null)
            {
                return Result.Failure<BookingFullDTO>(BookingResults.NotFound);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure<BookingFullDTO>(error);
            }
            return Result.Success(booking.MapToFullDTO());
        }
        public async Task<Result<BookingDTO>> UpdateNote(int bookingId, UpdateBookingNoteRequest request)
        {
            var booking = await _bookingRepository.GetWithBranch(bookingId);
            if (booking == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.NotFound);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.Id, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (error != Error.None)
            {
                return Result.Failure<BookingDTO>(error);
            }
            var oldNote = booking.Note;

            booking.Note = request.Note;
            booking.UpdateTimestamp();
            var changes = new List<BookingFieldChange>();

            AddChange(
                changes,
                nameof(Booking.Note),
                oldNote,
                booking.Note);

            await _bookingHistoryWriter.Add(
                booking,
                ActionType.NoteUpdated,
                changes,
                GetCurrentBookingActor(booking.Id));

            await _unitOfWork.SaveChangesAsync();

            var updatedBookingDTO = booking.MapToDTO();

            await NotifyBookingAsync(
                booking,
                BookingNotificationResults.BookingNoteUpdated(booking.Reference),
                updatedBookingDTO);

            return Result.Success(updatedBookingDTO);
        }
        public async Task<Result<List<BookingHistoryDto>>> GetBookingHistoryAsync(
            int bookingId,
            CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetWithBranch(bookingId);
            if (booking == null)
            {
                return Result.Failure<List<BookingHistoryDto>>(BookingResults.NotFound);
            }
            var accessError = await _accessGuard.EnsureAccessToBooking(bookingId, booking.ClientID, booking.EmployeeID, booking.Branch.CompanyId);
            if (accessError != Error.None)
            {
                return Result.Failure<List<BookingHistoryDto>>(accessError);
            }
            var records = await _bookingHistoryRepository.GetAll(bookingId, cancellationToken);
            var history = records.Select(x => x.HistoryMapToEntity()).ToList();

            return Result.Success(history);
        }
        private async Task NotifyCompanyBookingAsync(
            int companyId,
            string type,
            string title,
            string message,
            BookingDTO booking)
        {
            await _realtimeNotificationService.SendToCompanyAsync(
                companyId,
                new RealtimeNotificationPayload
                {
                    Type = type,
                    Title = title,
                    Message = message,
                    Data = booking
                });
        }
        private async Task NotifyBookingAsync(
            Booking booking,
            NotificationResult notificationResult,
            BookingDTO? bookingDTO = null)
        {
            bookingDTO ??= booking.MapToDTO();
            var notification = new RealtimeNotificationPayload
            {
                Type = notificationResult.Type,
                Title = notificationResult.Title,
                Message = notificationResult.Message,
                Data = bookingDTO
            };

            await _realtimeNotificationService.SendToUserAsync(booking.EmployeeID, notification);

            if (booking.ClientID.HasValue)
            {
                await _realtimeNotificationService.SendToUserAsync(booking.ClientID.Value, notification);
            }
        }

        private async Task<Error> ValidateCreateRequest(Service? service, UserAccount employee, DateTime startTime, int? clientUserAccountId, int? bookingId = null)
        {
            if (service == null)
            {
                return BookingResults.ServiceDoesntExists;
            }
            bool employeeHasService = employee.EmployeeServices.Any(e => e.ServiceId == service.Id);
            if (!employeeHasService)
            {
                return BookingResults.EmployeeServiceDoesntExists;
            }

            if (!employee.IsAvailable(startTime))
            {
                return BookingResults.EmployeeNotAvailable;
            }

            if (startTime <= DateTime.UtcNow)
            {
                return BookingResults.InvalidStartTime;
            }

            var endTimeExpected = startTime.AddMinutes(service.Duration);

            if (clientUserAccountId != null)
            {
                var clientConflict = await _bookingRepository.HasBookingOverlap(clientUserAccountId.Value, startTime, endTimeExpected, bookingId, asEmployee: false);
                if (clientConflict)
                    return BookingResults.ClientAlreadyBooked;
            }

            var employeeConflict = await _bookingRepository.HasBookingOverlap(employee.Id, startTime, endTimeExpected, bookingId, asEmployee: true);
            if (employeeConflict)
                return BookingResults.EmployeeAlreadyBooked;

            return Error.None;
        }
        private async Task<Result<UserAccount>> GetValidEmployee(int employeeLoginId)
        {
            var employee = await _userAccountRepository.GetEmployeeByUserLoginDataIDWithBookingData(employeeLoginId);
            if (employee == null || employee.CompanyID == null)
                return Result.Failure<UserAccount>(BookingResults.EmployeeDoesntExists);

            return Result.Success(employee);
        }

        private static void AddChange<T>(
            ICollection<BookingFieldChange> changes,
            string field,
            T oldValue,
            T newValue)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return;

            changes.Add(new BookingFieldChange(
                field,
                FormatHistoryValue(oldValue),
                FormatHistoryValue(newValue)));
        }

        private static string? FormatHistoryValue<T>(T value)
        {
            if (value is null)
                return null;

            return value switch
            {
                DateTime dateTime =>
                    dateTime.ToUniversalTime().ToString("O"),

                DateTimeOffset dateTimeOffset =>
                    dateTimeOffset.ToUniversalTime().ToString("O"),

                Enum enumValue =>
                    enumValue.ToString(),

                _ => Convert.ToString(
                    value,
                    System.Globalization.CultureInfo.InvariantCulture)
            };
        }
        private BookingActor GetCurrentBookingActor(int? bookingId = null)
        {
            if (bookingId.HasValue &&
                _authService.IsGuestForBooking(bookingId.Value))
            {
                return new BookingActor(
                    null,
                    BookingChangeSource.Guest);
            }

            var userId = _authService.GetUserAccountID();

            var source = _authService.IsInRole("Admin")
                ? BookingChangeSource.Administrator
                : BookingChangeSource.User;

            return new BookingActor(
                userId,
                source);
        }
    }
}
