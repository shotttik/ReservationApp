namespace Application.Common.Notifications
{
    public class BookingNotificationResults
    {
        public static NotificationResult BookingCreatedByGuest(string serviceName) => new NotificationResult
        {
            Type = "booking.created",
            Title = "New Booking Created",
            Message = $"A guest booking was created for {serviceName}."
        };
        public static NotificationResult BookingCreatedByClient(string serviceName) => new NotificationResult
        {
            Type = "booking.created",
            Title = "New Booking Created",
            Message = $"A client booking was created for {serviceName}."
        };
        public static NotificationResult BookingCreatedByAdmin(string serviceName) => new NotificationResult
        {
            Type = "booking.created",
            Title = "New Booking Created",
            Message = $"An admin booking was created for {serviceName}."
        };
        public static NotificationResult BookingStatusChanged(string bookingRef, string newStatus) => new NotificationResult
        {
            Type = "booking.status.changed",
            Title = "Booking Status Changed",
            Message = $"The booking {bookingRef} has changed status to {newStatus}."
        };
        public static NotificationResult BookingCancelled(string bookingRef, string serviceName) => new NotificationResult
        {
            Type = "booking.cancelled",
            Title = "Booking Cancelled",
            Message = $"The booking {bookingRef} for {serviceName} has been cancelled."
        };

        public static NotificationResult BookingRescheduled(string bookingRef, DateTime dateTime) => new NotificationResult
        {
            Type = "booking.rescheduled",
            Title = "Booking Rescheduled",
            Message = $"The booking {bookingRef} has been rescheduled for {dateTime}."
        };
        public static NotificationResult BookingNoteUpdated(string bookingRef) => new NotificationResult
        {
            Type = "booking.noteUpdated",
            Title = "Booking Note Updated",
            Message = $"The note for booking {bookingRef} has been updated."
        };
    }
}
