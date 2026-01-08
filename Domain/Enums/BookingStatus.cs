namespace Domain.Enums
{
    public enum BookingStatus
    {
        Failed = -2,
        Canceled = -1,
        PendingVerification = 0,
        Pending = 1,
        Accepted = 2,
        Completed = 3,
    }
}
