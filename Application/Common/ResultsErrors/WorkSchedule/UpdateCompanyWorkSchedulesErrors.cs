namespace Application.Common.ResultsErrors.WorkSchedule
{
    internal class UpdateCompanyWorkSchedulesErrors :WorkSchedulesErrors
    {
        public static readonly Error NotExists = Error.Validation(
            code: "UpdateCompanyWorkScheduleErrors.NotExists",
            description: "Work schedules does not exist for the company."
        );
        public static readonly Error Mismatch = Error.Validation(
            code: "UpdateCompanyWorkScheduleErrors.Mismatch",
            description: "Work schedules does not match with the company work schedules."
        );
    }
}