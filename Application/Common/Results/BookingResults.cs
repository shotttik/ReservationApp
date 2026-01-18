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
        public static readonly Error CompletedCantChange = Error.Validation("Booking.CompletedCantChange", "Completed bookings status cannot be changed.");
        public static readonly Error SameStatus = Error.Conflict("Booking.SameStatus", "Booking already have same status.");
        public static readonly Error AlreadyVerified = Error.Conflict("Booking.AlreadyVerified", "Booking already verified.");
        public static readonly Error WaitingForVerification = Error.Validation("Booking.WaitingForVerification", "Booking verification code already send and waiting for verification.");
        public static readonly Error ClientDoesntExists = Error.Validation("Booking.ClientDoesntExists", "Client doesn't exists with given ClientId.");
        public static readonly Error ClientOrGuestInfoMustBeProvided = Error.Validation("Booking.ClientOrGuestInfoMustBeProvided", "Client or guest info must be provided.");
        public static readonly Error VerificationCodeExpired = Error.Validation("Booking.VerificationCodeExpired", "Booking verification code expired.");
        public static readonly Error VerificationCodeIsWrong = Error.Validation("Booking.VerificationCodeIsWrong", "Booking verification code is wrong.");
        public static readonly Error DoesntRequireVerification = Error.Validation("Booking.DoesntRequireVerification", "Booking does not require verification.");
        public static readonly Error DoesntExists = Error.Validation("Booking.DoesntExists", "Booking doesn't exists.");
        public static readonly Error NotValidForVerification = Error.Validation("Booking.NotValidForVerification", "Booking doesn't exists or is not valid for verification process.");
        public static readonly Error NotValidForGuestAccess = Error.Validation("Booking.NotValidForGuestAccess", "Booking doesn't exists or is not valid for guest access.");
        public static readonly Error IsNotCancelable = Error.Validation("Booking.IsNotCancelable", "Booking is not cancelable.");
        #endregion
        #region Success
        public static readonly SuccessInfo StatusChanged = new("Booking.StatusChanged", "Booking status changed successfully.");
        public static readonly SuccessInfo Deleted = new("Booking.Deleted", "Booking deleted successfully.");
        public static readonly SuccessInfo VerificationCodeSent = new("Booking.VerificationCodeSent", "Verification code sent successfully.");
        public static readonly SuccessInfo VerifiedSuccess = new("Booking.VerifiedSuccess", "Verified successfully.");
        public static readonly SuccessInfo Canceled = new("Booking.Canceled", "Booking canceled successfully.");
        #endregion
    }
}
