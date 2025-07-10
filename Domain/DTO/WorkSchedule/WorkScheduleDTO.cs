namespace Domain.DTO.WorkSchedule
{
    public class WorkScheduleDTO
    {
        public int ID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
    }
}
