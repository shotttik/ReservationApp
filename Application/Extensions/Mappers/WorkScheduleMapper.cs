using Domain.DTO.WorkSchedule;
using Domain.Entities.Common;

namespace Application.Extensions.Mappers
{
    public static class WorkScheduleMapper
    {
        public static WorkScheduleDTO MapToDTO(this WorkSchedule workSchedule)
        {
            return new WorkScheduleDTO
            {
                ID = workSchedule.ID,
                DayOfWeek = workSchedule.DayOfWeek,
                StartTime = workSchedule.StartTime,
                EndTime = workSchedule.EndTime,
            };
        }

    }
}
