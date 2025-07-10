using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.WorkSchedule
{
    public class WorkScheduleCreateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        [Required]
        public int UserID { get; set; } // userlogindataID
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
