using Application.Common.ResultsErrors;
using Application.DTOs.WorkSchedule;

namespace Application.Interfaces
{
    public interface IWorkScheduleService
    {
        Task<Result> AddCompanyWorkSchedules(AddWorkSchedulesRequest request);
        Task<Result> UpdateCompanyWorkSchedules(UpdateWorkSchedulesRequest request);
    }
}
