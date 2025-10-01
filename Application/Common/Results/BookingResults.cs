namespace Application.Common.Results
{
    public class BookingResults
    {
        #region Errors
        public static readonly Error EmployeeDoesntExists = Error.Validation("Booking.EmployeeDoesntExists", "Employee Doesn't exists.");
        public static readonly Error ServiceDoesntExists = Error.Validation("Booking.ServiceDoesntExists", "Service Doesn't exists.");
        public static readonly Error EmployeeNotAvailable = Error.Validation("Booking.EmployeeNotAvailable", "Employee is not available at the requested time.");
        public static readonly Error ClientAlreadyHasBookingAtThatTime = Error.Validation("Booking.ClientAlreadyHasBookingAtThatTime", "Client already has a booking at that time.");
        public static readonly Error EmployeeAlreadyHasBookingAtThatTime = Error.Validation("Booking.EmployeeAlreadyHasBookingAtThatTime", "Employee already has a booking at that time.");
        public static readonly Error ClientAlreadyBooked = Error.Validation("Booking.ClientAlreadyBooked", "Client already has a booking at that time.");
        public static readonly Error EmployeeAlreadyBooked = Error.Validation("Booking.EmployeeAlreadyBooked", "Employee already has a booking at that time.");
        public static readonly Error InvalidStartTime = Error.Validation("Booking.InvalidStartTime", "Booking must be for a future date and time.");
        public static readonly Error NotFound = Error.NotFound("Booking.NotFound", "Booking not found.");
        public static readonly Error CompletedCantChange = Error.Validation("BookingResults.CompletedCantChange", "Completed bookings status cannot be changed.");
        public static readonly Error SameStatus = Error.Conflict("BookingResults.SameStatus", "Booking already have same status.");
        #endregion
        #region Success
        public static readonly SuccessInfo StatusChanged = new("Booking.StatusChanged", "Booking status changed successfully.");
        public static readonly SuccessInfo Deleted = new("Booking.Deleted", "Booking deleted successfully.");
        #endregion
    }
}
