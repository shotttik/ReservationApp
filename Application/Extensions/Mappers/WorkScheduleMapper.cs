using Application.Common.Requests.WorkSchedule;
using Domain.DTO.WorkSchedule;
using Domain.Entities.Common;

namespace Application.Extensions.Mappers
{
    public static class WorkScheduleMapper
    {
        public static WorkScheduleDTO MapToDTO(this WorkSchedule entity)
        {
            return new WorkScheduleDTO
            {
                Id = entity.ID,
                DayOfWeek = entity.DayOfWeek,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
            };
        }

        public static WorkScheduleException MapToEntity(this WorkScheduleExceptionCreateRequest request)
        {
            return new WorkScheduleException
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Notes = request.Notes,
                Type = request.Type
            };
        }
        public static void MapToEntity(this WorkScheduleExceptionUpdateRequest request, WorkScheduleException entity)
        {
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.Notes = request.Notes;
            entity.Type = request.Type;
        }

        public static WorkScheduleExceptionDTO MapToDTO(this WorkScheduleException entity)
        {
            return new WorkScheduleExceptionDTO()
            {
                Id = entity.ID,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Notes = entity.Notes,
                Type = entity.Type,
                CreatedAt = entity.CreatedAt
            };
        }

    }
}
