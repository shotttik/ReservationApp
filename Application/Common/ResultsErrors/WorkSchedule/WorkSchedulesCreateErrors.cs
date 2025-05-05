namespace Application.Common.ResultsErrors.WorkSchedule
{
    internal class WorkSchedulesCreateErrors :WorkSchedulesErrors
    {
        public static readonly Error InvalidWorkScheduleCount = Error.Validation(
                code: "AddWorkSchedulesErrors.InvalidCount",
                description: "The number of work schedules must be 7, one for each day of the week."
            );
        public static readonly Error AlreadyExists = Error.Validation(
                code: "AddWorkSchedulesErrors.AlreadyExists",
                description: "Work schedule already exists, only update is available."
            );
    }
}