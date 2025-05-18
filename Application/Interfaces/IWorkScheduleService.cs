using Application.Common.Results;
using Application.DTOs.WorkSchedule;

namespace Application.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<Result> WorkSchedulesCreate(WorkSchedulesCreateRequest request, bool isForEmployee);
        Task<Result> WorkSchedulesUpdate(WorkSchedulesUpdateRequest request, bool isForEmployee);
    }
}
