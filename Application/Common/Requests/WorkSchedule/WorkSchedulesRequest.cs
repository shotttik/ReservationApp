using Domain.DTO.WorkSchedule;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.WorkSchedule
{
    public class WorkSchedulesRequest<T> where T : BaseWorkScheduleDTO
    {
        [Required]
        public List<T> WorkSchedules { get; set; } = [];
    }
    public class WorkSchedulesCreateRequest :WorkSchedulesRequest<WorkScheduleCreateDTO> { }

    public class WorkSchedulesUpdateRequest :WorkSchedulesRequest<WorkScheduleUpdateDTO> { }

    public class WorkScheduleCreateDTO :BaseWorkScheduleDTO
    {
    }
    public class WorkScheduleUpdateDTO :BaseWorkScheduleDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ID must be greater than 0.")]
        public int ID { get; set; }
    }
}
