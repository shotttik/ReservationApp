using Application.DTOs.WorkSchedule;
using Domain.Entities;

namespace Application.Extensions.Mappers
{
    public static class WorkScheduleMapper
    {
        public static WorkSchedule MapToEntity(this WorkScheduleUpdateDTO workSchedule)
        {
            return new WorkSchedule
            {
                ID = workSchedule.ID,
                DayOfWeek = workSchedule.DayOfWeek,
                StartTime = workSchedule.StartTime,
                EndTime = workSchedule.EndTime,
                IsWorkingDay = workSchedule.IsWorkingDay
            };
        }
        public static WorkSchedule MapToEntity(this WorkScheduleCreateDTO workSchedule)
        {
            return new WorkSchedule
            {
                DayOfWeek = workSchedule.DayOfWeek,
                StartTime = workSchedule.StartTime,
                EndTime = workSchedule.EndTime,
                IsWorkingDay = workSchedule.IsWorkingDay
            };
        }
    }
}
