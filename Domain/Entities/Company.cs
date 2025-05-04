namespace Domain.Entities
{
    public class Company :BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string IN { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public ICollection<UserAccount> UserAccounts { get; set; } = [];
        public ICollection<CompanyInvitation> Invitations { get; set; } = [];
        public ICollection<Service> Services { get; set; } = [];
        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<WorkSchedule> WorkSchedules { get; set; } = [];
        public ICollection<WorkScheduleException> WorkScheduleExceptions { get; set; } = [];
    }
}
