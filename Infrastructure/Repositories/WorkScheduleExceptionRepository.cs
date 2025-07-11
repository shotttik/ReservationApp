using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class WorkScheduleExceptionRepository :BaseRepository<WorkScheduleException>, IWorkScheduleExceptionRepository
    {
        public WorkScheduleExceptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<WorkScheduleException>> GetAllForUser(int userId)
        {
            var workSchedules = await _dbSet.
                Where(e => e.UserAccount.UserLoginDataID == userId)
                .Include(e => e.UserAccount)
                .ToArrayAsync();

            return workSchedules;
        }
    }
}
