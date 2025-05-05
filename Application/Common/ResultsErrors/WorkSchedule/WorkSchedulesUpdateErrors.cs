namespace Application.Common.ResultsErrors.WorkSchedule
{
    internal class WorkSchedulesUpdateErrors :WorkSchedulesErrors
    {
        public static readonly Error NotExists = Error.Validation(
            code: "UpdateCompanyWorkScheduleErrors.NotExists",
            description: "Work schedules does not exist."
        );
        public static readonly Error Mismatch = Error.Validation(
            code: "UpdateCompanyWorkScheduleErrors.Mismatch",
            description: "Work schedules does not match with existed work schedules."
        );
    }
}