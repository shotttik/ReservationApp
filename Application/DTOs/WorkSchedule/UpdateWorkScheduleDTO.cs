using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.WorkSchedule
{
    public class UpdateWorkScheduleDTO :BaseWorkScheduleDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
