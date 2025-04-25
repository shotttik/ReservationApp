namespace Domain.Entities
{
    public class WorkSchedule
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int? UserID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool IsWorkingDay { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Company Company { get; set; } = null!;
        public UserAccount? User { get; set; }
        public void UpdateTimestamp() => UpdatedAt = DateTime.Now;

    }
}
