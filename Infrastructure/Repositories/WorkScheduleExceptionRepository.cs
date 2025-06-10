using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class WorkScheduleExceptionRepository :BaseRepository<WorkScheduleException>, IWorkScheduleExceptionRepository
    {
        public WorkScheduleExceptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
