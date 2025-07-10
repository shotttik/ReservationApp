using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    internal class WorkScheduleRepository :BaseRepository<WorkSchedule>, IWorkScheduleRepository
    {
        public WorkScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<WorkSchedule>> GetAllForUser(int userId)
        {
            var workSchedules = await _dbSet.
                Where(e => e.UserAccount.UserLoginDataID == userId)
                .Include(e => e.UserAccount)
                .ToArrayAsync();

            return workSchedules;
        }
    }
}
