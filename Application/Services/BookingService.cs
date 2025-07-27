using Application.Common.Requests.Booking;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class BookingService :IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IAuthService authService;
        private readonly IUserAccountRepository userAccountRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IAuthService authService,
            IUserAccountRepository userAccountRepository)
        {
            this.bookingRepository = bookingRepository;
            this.authService = authService;
            this.userAccountRepository = userAccountRepository;
        }

        public async Task<Result<BookingDTO>> Create(BookingCreateRequest request)
        {
            var authUser = await authService.GetCurrentUser();
            var userAccountID = authService.GetUserAccountID();
            var employee = await userAccountRepository.GetByUserLoginDataIDWithBookingData(request.EmployeeID);
            var isInvalidEmployee = employee == null || employee.CompanyID == null;
            if (isInvalidEmployee)
            {
                return Result.Failure<BookingDTO>(BookingResults.EmployeeDoesntExists);
            }

            var service = employee!.Company!.Services.FirstOrDefault(s => s.ID == request.ServiceID);
            if (service == null)
            {
                return Result.Failure<BookingDTO>(BookingResults.ServiceDoesntExists);
            }

            if (!employee.IsAvailable(request.StartTime))
            {
                return Result.Failure<BookingDTO>(BookingResults.EmployeeNotAvailable);
            }

            if (request.StartTime <= DateTime.UtcNow)
            {
                return Result.Failure<BookingDTO>(BookingResults.InvalidStartTime);
            }

            var endTimeExpected = request.StartTime.AddMinutes(service.Duration);


            var clientConflict = await bookingRepository.HasBookingOverlap(userAccountID, request.StartTime, endTimeExpected, asEmployee: false);
            if (clientConflict)
                return Result.Failure<BookingDTO>(BookingResults.ClientAlreadyBooked);

            var employeeConflict = await bookingRepository.HasBookingOverlap(employee.ID, request.StartTime, endTimeExpected, asEmployee: true);
            if (employeeConflict)
                return Result.Failure<BookingDTO>(BookingResults.EmployeeAlreadyBooked);

            var booking = request.MapToEntity(service);
            booking.ClientID = userAccountID;
            booking.CompanyID = employee.CompanyID!.Value;
            booking.EmployeeID = employee.ID;
            await bookingRepository.Add(booking);

            var bookingDTO = booking.MapToDTO();

            return Result.Success(bookingDTO);
        }
    }
}
