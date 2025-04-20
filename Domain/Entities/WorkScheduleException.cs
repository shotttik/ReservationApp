namespace Domain.Entities
{
    public class WorkScheduleException
    {
        public int ID { get; set; }
        public int CompanyID { get; set; }
        public int? UserAccountID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Reason { get; set; }
        public bool IsFullDay { get; set; }
        public Company Company { get; set; } = null!;
        public UserAccount? UserAccount { get; set; }

    }
}
