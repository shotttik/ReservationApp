namespace Domain.Entities
{
    public class WorkSchedule :BaseEntity
    {
        public int CompanyID { get; set; }
        public int? UserID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public TimeOnly? BreakStartTime { get; set; }
        public TimeOnly? BreakEndTime { get; set; }
        public bool IsWorkingDay { get; set; }

        // Navigation properties
        public Company Company { get; set; } = null!;
        public UserAccount? User { get; set; }

        // Computed properties
        public bool Is24HourShift => IsWorkingDay &&
                                     StartTime.HasValue &&
                                     EndTime.HasValue &&
                                     StartTime == EndTime;

        public bool IsOvernightShift => IsWorkingDay &&
                                        StartTime.HasValue &&
                                        EndTime.HasValue &&
                                        EndTime < StartTime &&
                                        !Is24HourShift;

        public TimeSpan WorkingHours
        {
            get
            {
                if (!IsWorkingDay || !StartTime.HasValue || !EndTime.HasValue)
                    return TimeSpan.Zero;

                if (Is24HourShift)
                    return TimeSpan.FromHours(24);

                if (IsOvernightShift)
                {
                    var hoursUntilMidnight = TimeSpan.FromHours(24) - StartTime.Value.ToTimeSpan();
                    var hoursAfterMidnight = EndTime.Value.ToTimeSpan();
                    return hoursUntilMidnight + hoursAfterMidnight;
                }

                return EndTime.Value.ToTimeSpan() - StartTime.Value.ToTimeSpan();
            }
        }

        public TimeSpan? BreakDuration
        {
            get
            {
                if (!BreakStartTime.HasValue || !BreakEndTime.HasValue)
                    return null;

                return BreakEndTime.Value.ToTimeSpan() - BreakStartTime.Value.ToTimeSpan();
            }
        }

        public TimeSpan NetWorkingHours
        {
            get
            {
                var workingHours = WorkingHours;
                var breakDuration = BreakDuration ?? TimeSpan.Zero;
                return workingHours - breakDuration;
            }
        }
    }
}

