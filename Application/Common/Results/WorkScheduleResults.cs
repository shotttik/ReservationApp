using Application.Common.ResultsErrors;

namespace Application.Common.Results
{
    public class WorkScheduleResults
    {
        #region Errors
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
        public static readonly Error InvalidWorkScheduleCount = Error.Validation(
        code: "WorkSchedule.DaysInvalidCount",
        description: "The number of work schedules must be 7, one for each day of the week."
        );
        public static readonly Error AlreadyExists = Error.Validation(
                code: "WorkSchedule.AlreadyExists",
                description: "Work schedule already exists, only update is available."
        );
        public static readonly Error NotExists = Error.Validation(
            code: "WorkSchedule.NotExists",
            description: "Work schedules does not exist."
        );
        public static readonly Error Mismatch = Error.Validation(
            code: "WorkSchedule.Mismatch",
            description: "Work schedules does not match with existed work schedules."
        );
        #endregion
    }
}
