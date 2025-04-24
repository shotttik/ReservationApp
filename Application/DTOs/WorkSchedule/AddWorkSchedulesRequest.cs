using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.WorkSchedule
{
    public class WorkSchedulesRequest
    {
        [Required]
        public List<WorkScheduleDTO> WorkSchedules { get; set; } = [];
    }
}
