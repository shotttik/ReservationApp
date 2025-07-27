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
        public bool IsWithinSchedule(DateTime dateTime)
        {
            // Check if the schedule is for the same day of week
            if (dateTime.DayOfWeek != DayOfWeek)
                return false;

            if (StartTime == null || EndTime == null)
                return false;

            var time = TimeOnly.FromDateTime(dateTime);

            // Handle overnight schedules (e.g., 22:00 - 06:00)
            if (EndTime < StartTime)
            {
                // If time is after StartTime or before EndTime
                return time >= StartTime || time <= EndTime;
            }
            else
            {
                return time >= StartTime && time < EndTime;
            }
        }
    }
}

