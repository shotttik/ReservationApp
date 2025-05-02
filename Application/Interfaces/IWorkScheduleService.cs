using Application.Common.ResultsErrors;
using Application.DTOs.WorkSchedule;

namespace Application.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<Result> AddWorkSchedules(CreateWorkSchedulesRequest request, bool isForEmployee);
        Task<Result> UpdateWorkSchedules(UpdateWorkSchedulesRequest request, bool isForEmployee);
    }
}
