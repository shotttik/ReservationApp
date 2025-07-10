using Domain.Entities.User;

namespace Domain.Entities.Common
{
    public class WorkSchedule :BaseEntity
    {
        public int UserAccountID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public virtual UserAccount UserAccount { get; set; } = null!;

    }
}

