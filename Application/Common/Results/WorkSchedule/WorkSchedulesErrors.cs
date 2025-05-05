namespace Application.Common.ResultsErrors.WorkSchedule
{
    internal partial class WorkSchedulesErrors
    {
        public static readonly Error UserMentioned = Error.Validation(
            code: "WorkSchedule.UserMentioned",
            description: "User cannot be mentioned in company work schedule."
        );
        public static readonly Error InvalidStartEndTime = Error.Validation(
            code: "WorkSchedule.InvalidStartEndTime",
            description: "Start time must be less than end time."
        );

        // is its not working day , start and end time must not be provided  
        public static readonly Error NonWorkingDay = Error.Validation(
            code: "WorkSchedule.NonWorkingDay",
            description: "Start and end time must not be provided for non-working days."
        );

        public static readonly Error EmployeeWorkingTimesOutOfBounds = Error.Validation(
            code: "WorkSchedule.EmployeeWorkingTimesOutOfBounds",
            description: "Employee working times are out of bounds of company work schedule."
        );
    }
}