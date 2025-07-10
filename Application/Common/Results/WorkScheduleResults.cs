namespace Application.Common.Results
{
    public class WorkScheduleResults
    {
        #region Errors
        public static readonly Error DoesntExists = Error.Validation("WorkSchedule.DoesntExists", "Work schedule doesn't exists.");
        public static readonly Error InvalidTimeRange = Error.Validation("WorkSchedule.InvalidTimeRange", "Start time cannot be greater than end time.");
        public static readonly Error OverlappingSchedule = Error.Validation("WorkSchedule.OverlappingSchedule", "The schedule overlaps with another schedule.");
        #endregion
        #region Success
        public static readonly SuccessInfo Created = new SuccessInfo("WorkSchedule.Created", "Work schedule created successfully.");
        public static readonly SuccessInfo Updated = new SuccessInfo("WorkSchedule.Updated", "Work schedule updated successfully.");
        public static readonly SuccessInfo Deleted = new SuccessInfo("WorkSchedule.Deleted", "Work schedule deleted successfully.");
        #endregion
    }
}
