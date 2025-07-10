using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.WorkSchedule
{
    public class WorkScheduleUpdateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        [Required]
        public int ID { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        [Required]
        public int UserID { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
