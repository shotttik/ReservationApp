using Application.Common.ResultsErrors;
using Application.DTOs.WorkSchedule;

namespace Application.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<Result> AddCompanyWorkSchedules(WorkSchedulesRequest schedulesRequest);
        Task<Result> UpdateCompanyWorkSchedules(WorkSchedulesRequest request);
    }
}
