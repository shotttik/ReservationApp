using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.WorkSchedule
{
    public class WorkScheduleExceptionUpdateRequest
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public WorkScheduleExceptionType Type { get; set; }

        public string? Notes { get; set; }
    }
}
