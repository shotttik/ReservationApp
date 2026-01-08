using Application.Common.Requests.Booking;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Extensions.Mappers.Pagination;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Entities.User;
using Domain.Enums;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class BookingService :IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IAuthService _authService;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IAccessGuard _accessGuard;
        private readonly IBookingVerificationService _bookingVerificationService;


        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserAccountRepository userAccountRepository,
            IAccessGuard accessGuard,
            IBookingVerificationService bookingVerificationService
            )
        {
            _bookingRepository = bookingRepository;
            _authService = authService;
            _userAccountRepository = userAccountRepository;
            _accessGuard = accessGuard;
            _bookingVerificationService = bookingVerificationService;
        }

        public async Task<Result<BookingDTO>> CreateByClient(ClientBookingCreateRequest request)
        {
            var authUser = await _authService.GetCurrentUser();

            var employeeResult = await GetValidEmployee(request.EmployeeID);
            if (!employeeResult.IsSuccess)
                return Result.Failure<BookingDTO>(employeeResult.Error);
            var employee = employeeResult.Value!;

            var service = employee!.Company!.Services.FirstOrDefault(s => s.ID == request.ServiceID);
            if (service == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.ServiceDoesntExists);
            }

            var requestValidationError = await ValidateCreateRequest(service, employee, request, authUser.UserAccountId);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }

            var booking = request.MapToEntity(service, authUser.UserAccountId, employee.CompanyID!.Value, employee.ID);
            await _bookingRepository.Add(booking);

            var bookingDTO = booking.MapToDTO();

            return Result.Success(bookingDTO);
        }

        public async Task<Result<BookingDTO>> CreateByAdmin(AdminBookingCreateRequest request)
        {
            var employeeResult = await GetValidEmployee(request.EmployeeID);
            if (!employeeResult.IsSuccess)
                return Result.Failure<BookingDTO>(employeeResult.Error);
            var employee = employeeResult.Value!;
            var companyID = employee.CompanyID!.Value;

            var accessError = await _accessGuard.EnsureAccessToCompanyEmployee(companyID, employee.UserLoginDataID);
            if (accessError != Error.None)
            {
                return Result.Failure<BookingDTO>(accessError);
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
            var service = employee.Company!.Services.FirstOrDefault(s => s.ID == request.ServiceID);
            if (service == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.ServiceDoesntExists);
            }
            var requestValidationError = await ValidateCreateRequest(service, employee, request, request.ClientId);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }

            var booking = request.MapToEntity(service, request.ClientId, companyID, employee.ID);

            booking.Status = BookingStatus.Accepted;
            if (bookingGuestInfo != null && bookingVerification != null)
            {
                booking.GuestInfo = bookingGuestInfo;
                booking.Verifications.Add(bookingVerification);
                booking.Status = BookingStatus.PendingVerification;
            }
            await _bookingRepository.Add(booking);

            if (bookingGuestInfo != null)
            {
                await _bookingVerificationService.SendVerificationNotification(
                    bookingGuestInfo.ContactType,
                    bookingGuestInfo.Contact,
                    bookingGuestInfo.DisplayName,
                    code);
            }
            var bookingDTO = booking.MapToDTO();

            return Result.Success(bookingDTO);
        }
        public async Task<Result<List<BookingDTO>>> GetWeeklyPublicData(int companyId, DateOnly targetDate)
        {
            var endDate = targetDate.AddDays(7);
            var data = await _bookingRepository.GetDataForAllActiveEmployees(companyId, targetDate, endDate);
            var bookings = data.Select(e => e.MapToDTO()).ToList();

            return Result.Success(bookings);
        }
        public async Task<Result> ChangeStatus(int bookingId, BookingStatusChangeRequest request)
        {
            var booking = await _bookingRepository.Get(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            var error = await _accessGuard.EnsureAccessToBooking(booking.ClientID, booking.EmployeeID, booking.CompanyID);
            if (error != Error.None)
            {
                return Result.Failure(error);
            }
            if (booking.Status == request.Status)
            {
                return Result.Failure(BookingResults.SameStatus);
            }
            booking.Status = request.Status;
            if (request.IsCompleted)
            {
                booking.EndTime = DateTime.Now;
            }
            if (request.IsCanceled || request.IsFailed)
            {
                booking.CancellationReason = request.CancellationReason;
            }
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
        private async Task<Error> ValidateCreateRequest(Service? service, UserAccount employee, ClientBookingCreateRequest request, int? clientUserAccountId)
        {
            if (service == null)
            {
                return BookingResults.ServiceDoesntExists;
            }

            if (!employee.IsAvailable(request.StartTime))
            {
                return BookingResults.EmployeeNotAvailable;
            }

            if (request.StartTime <= DateTime.UtcNow)
            {
                return BookingResults.InvalidStartTime;
            }

            var endTimeExpected = request.StartTime.AddMinutes(service.Duration);

            if (clientUserAccountId != null)
            {
                var clientConflict = await _bookingRepository.HasBookingOverlap(clientUserAccountId.Value, request.StartTime, endTimeExpected, asEmployee: false);
                if (clientConflict)
                    return BookingResults.ClientAlreadyBooked;
            }

            var employeeConflict = await _bookingRepository.HasBookingOverlap(employee.ID, request.StartTime, endTimeExpected, asEmployee: true);
            if (employeeConflict)
                return BookingResults.EmployeeAlreadyBooked;

            return Error.None;
        }
        private async Task<Result<UserAccount>> GetValidEmployee(int employeeLoginId)
        {
            var employee = await _userAccountRepository.GetByUserLoginDataIDWithBookingData(employeeLoginId);
            if (employee == null || employee.CompanyID == null)
                return Result.Failure<UserAccount>(BookingResults.EmployeeDoesntExists);

            return Result.Success(employee);
        }
    }
}
