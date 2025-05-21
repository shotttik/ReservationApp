using Domain.DTO.WorkSchedule;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.WorkSchedule
{
    public class WorkSchedulesRequest<T> where T : BaseWorkScheduleDTO
    {
        [Required]
        public List<T> WorkSchedules { get; set; } = [];
    }
    public class WorkSchedulesCreateRequest :WorkSchedulesRequest<WorkScheduleCreateDTO> { }

    public class WorkSchedulesUpdateRequest :WorkSchedulesRequest<WorkScheduleUpdateDTO> { }

}
