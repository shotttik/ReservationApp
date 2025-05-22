using Domain.Enums;

namespace Domain.Entities
{
    public class UserAccount :BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? CompanyID { get; set; }
        public required int RoleID { get; set; }
        public virtual UserLoginData? UserLoginData { get; set; }
        public virtual Role? Role { get; set; }
        public virtual Company? Company { get; set; }
        public ICollection<Appointment> AppointmentsAsClient { get; set; } = [];
        public ICollection<Appointment> AppointmentsAsEmployee { get; set; } = [];
        public ICollection<WorkSchedule> WorkSchedules { get; set; } = [];
        public ICollection<WorkScheduleException> WorkScheduleExceptions { get; set; } = [];
    }
}
