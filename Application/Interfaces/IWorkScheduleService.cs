using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;

namespace Application.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<Result> WorkSchedulesCreate(WorkSchedulesCreateRequest request, bool isForEmployee);
        Task<Result> WorkSchedulesUpdate(WorkSchedulesUpdateRequest request, bool isForEmployee);
    }
}
