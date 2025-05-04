namespace Domain.Entities
{
    public class WorkSchedule :BaseEntity
    {
        public int CompanyID { get; set; }
        public int? UserID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool IsWorkingDay { get; set; }
        public Company Company { get; set; } = null!;
        public UserAccount? User { get; set; }
    }
}
