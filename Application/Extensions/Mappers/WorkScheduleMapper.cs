using Application.Common.Requests.WorkSchedule;
using Domain.DTO.WorkSchedule;
using Domain.Entities.Common;

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
                BreakStartTime = workSchedule.BreakStartTime,
                BreakEndTime = workSchedule.BreakEndTime,
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
                BreakStartTime = workSchedule.BreakStartTime,
                BreakEndTime = workSchedule.BreakEndTime,
                IsWorkingDay = workSchedule.IsWorkingDay
            };
        }
        public static WorkScheduleDTO MapToDTO(this WorkSchedule workSchedule)
        {
            return new WorkScheduleDTO
            {
                ID = workSchedule.ID,
                CompanyID = workSchedule.CompanyID,
                UserID = workSchedule.UserID,
                DayOfWeek = workSchedule.DayOfWeek,
                StartTime = workSchedule.StartTime,
                EndTime = workSchedule.EndTime,
                BreakStartTime = workSchedule.BreakStartTime,
                BreakEndTime = workSchedule.BreakEndTime,
                IsWorkingDay = workSchedule.IsWorkingDay
            };
        }

    }
}
