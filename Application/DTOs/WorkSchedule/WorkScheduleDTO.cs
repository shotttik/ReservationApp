namespace Application.DTOs.WorkSchedule
{
    public class WorkScheduleDTO :UpdateWorkScheduleDTO
    {
        public int CompanyID { get; set; }
        public int? UserID { get; set; }

    }
}
