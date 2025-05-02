using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.WorkSchedule
{
    public class WorkSchedulesRequest<T> where T : BaseWorkScheduleDTO
    {
        [Required]
        public List<T> WorkSchedules { get; set; } = [];
    }
    public class CreateWorkSchedulesRequest :WorkSchedulesRequest<CreateWorkScheduleDTO> { }

    public class UpdateWorkSchedulesRequest :WorkSchedulesRequest<UpdateWorkScheduleDTO> { }

}
