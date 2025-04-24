using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.WorkSchedule
{
    public class WorkSchedulesRequest<T> where T : BaseWorkScheduleDTO
    {
        [Required]
        public List<T> WorkSchedules { get; set; } = [];
    }
    public class AddWorkSchedulesRequest :WorkSchedulesRequest<AddWorkScheduleDTO> { }

    public class UpdateWorkSchedulesRequest :WorkSchedulesRequest<UpdateWorkScheduleDTO> { }

}
