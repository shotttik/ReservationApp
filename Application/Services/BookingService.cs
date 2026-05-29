using Application.Authentication;
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
        private readonly IGuestBookingService _bookingVerificationService;
        private readonly BookingSettings _bookingSettings;
        private readonly ISubscriptionGuard _subscriptionGuard;
        private readonly IPromoService _promoService;
        private readonly IPromoCodeRepository _promoCodeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserAccountRepository userAccountRepository,
            IAccessGuard accessGuard,
            IGuestBookingService bookingVerificationService,
            IOptions<BookingSettings> bookingSettings,
            ISubscriptionGuard subscriptionGuard,
            IPromoService promoService,
            IPromoCodeRepository promoCodeRepository,
            IUnitOfWork unitOfWork
            )
        {
            _bookingRepository = bookingRepository;
            _authService = authService;
            _userAccountRepository = userAccountRepository;
            _accessGuard = accessGuard;
            _bookingVerificationService = bookingVerificationService;
            _bookingSettings = bookingSettings.Value;
            _subscriptionGuard = subscriptionGuard;
            _promoService = promoService;
            _promoCodeRepository = promoCodeRepository;
            _unitOfWork = unitOfWork;
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

            decimal finalPrice = booking.PriceFull;
            PromoCode? appliedPromo = null;

            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promoResult = await _promoService.ApplyPromo(request.PromoCode, companyId, booking.PriceFull);

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
            (var bookingVerification, var code) = _bookingVerificationService.CreateBookingVerification(request.GuestInfo.ContactType);
            booking.Status = BookingStatus.PendingVerification;
            booking.GuestInfo = bookingGuestInfo;
            booking.Verifications.Add(bookingVerification);

            await _bookingRepository.Add(booking);
            if (appliedPromo != null)
            {
                appliedPromo.UsedCount++;
                await _promoCodeRepository.Update(appliedPromo);
            }
            await _bookingVerificationService.SendVerificationNotification(
                bookingGuestInfo.ContactType,
                bookingGuestInfo.Contact,
                bookingGuestInfo.DisplayName,
                code,
                booking.Reference);

            var token = JWTGenerator.GenerateGuestToken(booking.Id, _bookingSettings);
            var bookingDTO = booking.MapToDTO();
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
            decimal finalPrice = booking.PriceFull;
            PromoCode? appliedPromo = null;

            if (!string.IsNullOrEmpty(request.PromoCode))
            {
                var promoResult = await _promoService.ApplyPromo(request.PromoCode, companyId, booking.PriceFull);

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

            return Result.Success(bookingDTO);
        }

        public async Task<Result<BookingDTO>> CreateByAdmin(AdminBookingCreateRequest request)
        {
            await _unitOfWork.BeginTransactionAsync();
            BookingDTO bookingDTO;
            try
            {

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

                    (bookingVerification, code) = _bookingVerificationService.CreateBookingVerification(request.GuestInfo.ContactType);
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
                decimal finalPrice = booking.PriceFull;
                PromoCode? appliedPromo = null;

                if (!string.IsNullOrEmpty(request.PromoCode))
                {
                    var promoResult = await _promoService.ApplyPromo(request.PromoCode, companyId, booking.PriceFull);

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
                    await _bookingVerificationService.SendVerificationNotification(
                        bookingGuestInfo.ContactType,
                        bookingGuestInfo.Contact,
                        bookingGuestInfo.DisplayName,
                        code,
                        booking.Reference);
                }
                bookingDTO = booking.MapToDTO();
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
            if (request.IsCompleted)
            {
                booking.EndTime = DateTime.Now;
                booking.PriceFinal = booking.PriceFull - booking.Discount;
            }
            if (request.IsCanceled || request.IsFailed)
            {
                booking.CancellationReason = request.CancellationReason;
            }
            booking.Status = request.Status;
            await _bookingRepository.Update(booking);

            return Result.Success(BookingResults.StatusChanged);
        }
        public async Task<Result<PagedList<BookingDTO>>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken)
        {
            var allowedFields = BookingFieldMap.DtoToEntityPath;
            var errors = parameters.Validate(allowedFields, typeof(Booking));

            if (errors.Any())
            {
                return Result.Failure<PagedList<BookingDTO>>(PagedListResults.InvalidPagedParameters(errors.First()));
            }

            var bookings = await _bookingRepository.RetrievePaged(
                parameters,
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
            booking.Cancel(request?.CancellationReason);
            await _bookingRepository.Update(booking);

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

            booking.Service = service;
            booking.ServiceID = service.Id;
            booking.Employee = employee;
            booking.EmployeeID = employee.Id;
            booking.StartTime = request.StartTime;
            booking.UpdateEndTimeExpected();
            booking.Status = BookingStatus.Pending;
            booking.UpdateTimestamp();

            await _bookingRepository.Update(booking);

            return booking.MapToDTO();
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
    }
}
