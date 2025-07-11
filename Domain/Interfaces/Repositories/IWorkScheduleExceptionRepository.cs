using Domain.Entities.Common;

namespace Domain.Interfaces.Repositories
{
    public interface IWorkScheduleExceptionRepository :IBaseRepository<WorkScheduleException>
    {
        Task<IEnumerable<WorkScheduleException>> GetAllForUser(int userId);
    }
}
