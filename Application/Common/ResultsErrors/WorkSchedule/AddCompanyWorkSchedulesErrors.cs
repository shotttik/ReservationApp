namespace Application.Common.ResultsErrors.WorkSchedule
{
    internal partial class WorkSchedulesErrors
    {
        internal class AddCompanyWorkSchedulesErrors :WorkSchedulesErrors
        {
            public static readonly Error InvalidWorkScheduleCount = Error.Validation(
                    code: "AddCompanyWorkSchedule.InvalidCount",
                    description: "The number of work schedules must be 7, one for each day of the week."
                );
            public static readonly Error AlreadyExists = Error.Validation(
                    code: "AddCompanyWorkSchedule.AlreadyExists",
                    description: "Work schedule already exists for the company, only update is available."
                );
        }
    }
}