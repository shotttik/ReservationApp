using Domain.Entities.User;
using Domain.Enums;

namespace Domain.Entities.Common
{
    public class WorkScheduleException :BaseEntity
    {
        public int UserAccountID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public WorkScheduleExceptionType Type { get; set; }
        public string? Notes { get; set; }
        public virtual UserAccount UserAccount { get; set; } = null!;
    }
}
