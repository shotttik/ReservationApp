namespace Domain.DTO.WorkSchedule
{
    public abstract class BaseWorkScheduleDTO
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool IsWorkingDay { get; set; }
    }
}
