namespace Application.Common.Results
{
    public static class ServiceResults
    {
        public static readonly Error InvalidServiceForCompany = Error.Validation("Service.InvalidServiceForCompany", "Invalid service for company");
        public static readonly Error CannotRemoveServiceWithFutureBookings = Error.Validation("Service.CannotRemoveServiceWithFutureBookings", "Cannot remove service with future bookings");
    }
}
