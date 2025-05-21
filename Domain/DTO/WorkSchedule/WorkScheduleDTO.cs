namespace Domain.DTO.WorkSchedule
{
    public class WorkScheduleDTO :WorkScheduleUpdateDTO
    {
        public int CompanyID { get; set; }
        public int? UserID { get; set; }

    }
}
