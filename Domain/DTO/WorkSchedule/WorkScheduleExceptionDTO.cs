using Domain.Enums;

namespace Domain.DTO.WorkSchedule
{
    public class WorkScheduleExceptionDTO
    {
        public int Id { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public WorkScheduleExceptionType Type { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
