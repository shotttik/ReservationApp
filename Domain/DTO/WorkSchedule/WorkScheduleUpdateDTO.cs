using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.WorkSchedule
{
    public class WorkScheduleUpdateDTO :BaseWorkScheduleDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
