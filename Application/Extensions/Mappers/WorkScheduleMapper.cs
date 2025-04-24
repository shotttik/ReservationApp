using Application.DTOs.WorkSchedule;
using Domain.Entities;

namespace Application.Extensions.Mappers
{
    public static class WorkScheduleMapper
    {
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
                IsWorkingDay = workSchedule.IsWorkingDay
            };
        }
        public static WorkSchedule MapToEntity(this WorkScheduleDTO workSchedule)
        {
            return new WorkSchedule
            {
                ID = workSchedule.ID,
                CompanyID = workSchedule.CompanyID,
                UserID = workSchedule.UserID,
                DayOfWeek = workSchedule.DayOfWeek,
                StartTime = workSchedule.StartTime,
                EndTime = workSchedule.EndTime,
                IsWorkingDay = workSchedule.IsWorkingDay
            };
        }
    }
}
