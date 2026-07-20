using Domain.Entities.BranchReleated;
using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Enums;

namespace Domain.Entities.User
{
    public class UserAccount :BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? CompanyID { get; set; }
        public required int RoleID { get; set; }
        public int UserLoginDataID { get; set; }
        public virtual UserLoginData UserLoginData { get; set; } = null!;
        public virtual Role? Role { get; set; }
        public virtual Company? Company { get; set; }
        public int? BranchId { get; set; }
        public virtual Branch? Branch { get; set; }
        public ICollection<Booking> BookingsAsClient { get; set; } = [];
        public ICollection<Booking> BookingsAsEmployee { get; set; } = [];
        public ICollection<WorkSchedule> WorkSchedules { get; set; } = [];
        public ICollection<WorkScheduleException> WorkScheduleExceptions { get; set; } = [];
        public ICollection<UserAccountMedia> UserAccountMedia { get; set; } = [];
        public ICollection<EmployeeService> EmployeeServices { get; set; } = [];
        public ICollection<NotificationRecipient> NotificationRecipients { get; set; } = [];

        //is available by given date time (eg workschedule + exceptions)
        public bool IsAvailable(DateTime dateTime)
        {
            // Check if the user has any work schedules
            if (WorkSchedules.Count == 0)
            {
                return false; // No work schedules means not available
            }
            // Check if the dateTime falls within any of the work schedules
            foreach (var schedule in WorkSchedules)
            {
                if (schedule.IsWithinSchedule(dateTime))
                {
                    // Check for exceptions
                    foreach (var exception in WorkScheduleExceptions)
                    {
                        if (exception.IsActiveOn(dateTime))
                        {
                            return false; // Exception overrides availability
                        }
                    }
                    return true; // Available during this schedule
                }
            }
            return false; // Not available in any schedule
        }
    }
}
