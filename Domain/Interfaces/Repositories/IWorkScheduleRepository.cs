using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IWorkScheduleRepository :IBaseRepository<WorkSchedule>
    {
        Task<IEnumerable<WorkSchedule>> GetAllForUser(int userId);
    }
}
