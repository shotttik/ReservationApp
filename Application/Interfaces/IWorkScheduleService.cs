using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Domain.DTO.WorkSchedule;

namespace Application.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<Result> Create(WorkScheduleCreateRequest request);
        Task<Result> Update(WorkScheduleUpdateRequest request);
        Task<Result> Delete(int id);
        Task<Result<List<WorkScheduleDTO>>> GetAllForUser(int userId);
    }
}
