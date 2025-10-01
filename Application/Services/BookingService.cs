using Application.Common.Requests.Booking;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
using Domain.Entities.CompanyReleated;
using Domain.Entities.User;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class BookingService :IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IAuthService authService;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IAccessGuard accessGuard;

        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserAccountRepository userAccountRepository,
            IAccessGuard accessGuard)
        {
            this.bookingRepository = bookingRepository;
            this.authService = authService;
            this.userAccountRepository = userAccountRepository;
            this.accessGuard = accessGuard;
        }

        public async Task<Result<BookingDTO>> CreateByClient(ClientBookingCreateRequest request)
        {
            var authUser = await authService.GetCurrentUser();
            var clientUserAccountId = authService.GetUserAccountID();

            var employeeResult = await GetValidEmployee(request.EmployeeID);
            if (!employeeResult.IsSuccess)
                return Result.Failure<BookingDTO>(employeeResult.Error);
            var employee = employeeResult.Value!;

            var service = employee!.Company!.Services.FirstOrDefault(s => s.ID == request.ServiceID);

            var requestValidationError = await ValidateCreateRequest(service, employee, request, clientUserAccountId);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }

            var booking = request.MapToEntity(service!, clientUserAccountId, employee.CompanyID!.Value, employee.ID);
            await bookingRepository.Add(booking);

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

            var accessError = await accessGuard.EnsureAccessToCompanyEmployee(companyID, employee.UserLoginDataID);
            if (accessError != Error.None)
            {
                return Result.Failure<BookingDTO>(accessError);
            }

            var clientAccount = await userAccountRepository.GetByEmailWithClientBookingData(request.ClientEmail);

            var service = employee!.Company!.Services.FirstOrDefault(s => s.ID == request.ServiceID);

            var requestValidationError = await ValidateCreateRequest(service, employee, request, clientAccount?.ID);

            if (requestValidationError != Error.None)
            {
                return Result.Failure<BookingDTO>(requestValidationError);
            }

            var booking = request.MapToEntity(service!, clientAccount?.ID, companyID, employee.ID);
            // @TODO roca user ar aris daregisttirebuli shesacvlelia logika, savaraudod mobiluris nomerze unda gaketdes bookingi
            if (clientAccount == null)
            {
                booking.Note = string.Join('\n', new [] { booking.Note, $"Email: {request.ClientEmail}" }.Where(s => !string.IsNullOrWhiteSpace(s)));
            }
            await bookingRepository.Add(booking);

            var bookingDTO = booking.MapToDTO();

            return Result.Success(bookingDTO);
        }

        public async Task<Result<List<BookingDTO>>> GetWeeklyPublicData(int companyId, DateOnly targetDate)
        {
            var endDate = targetDate.AddDays(7);
            var data = await bookingRepository.GetDataForAllActiveEmployees(companyId, targetDate, endDate);
            var bookings = data.Select(e => e.MapToDTO()).ToList();

            return Result.Success(bookings);
        }

        public async Task<Result> ChangeStatus(int bookingId, BookingStatusChangeRequest request)
        {
            var booking = await bookingRepository.Get(bookingId);
            if (booking == null)
            {
                return Result.Failure(BookingResults.NotFound);
            }
            var error = await accessGuard.EnsureAccessToBooking(booking.ClientID, booking.EmployeeID, booking.CompanyID);
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
            await bookingRepository.Update(booking);

            return Result.Success(BookingResults.StatusChanged);
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
                var clientConflict = await bookingRepository.HasBookingOverlap(clientUserAccountId.Value, request.StartTime, endTimeExpected, asEmployee: false);
                if (clientConflict)
                    return BookingResults.ClientAlreadyBooked;
            }

            var employeeConflict = await bookingRepository.HasBookingOverlap(employee.ID, request.StartTime, endTimeExpected, asEmployee: true);
            if (employeeConflict)
                return BookingResults.EmployeeAlreadyBooked;

            return Error.None;
        }
        private async Task<Result<UserAccount>> GetValidEmployee(int employeeLoginId)
        {
            var employee = await userAccountRepository.GetByUserLoginDataIDWithBookingData(employeeLoginId);
            if (employee == null || employee.CompanyID == null)
                return Result.Failure<UserAccount>(BookingResults.EmployeeDoesntExists);

            return Result.Success(employee);
        }
    }
}
