using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.DTOs.WorkSchedule
{
    public class WorkScheduleDTO
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "CompanyID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CompanyID must be greater than 0.")]
        public int CompanyID { get; set; }
        public int? UserID { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public bool IsWorkingDay { get; set; }
    }
}
