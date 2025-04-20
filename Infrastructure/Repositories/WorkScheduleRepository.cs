using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    internal class WorkScheduleRepository :BaseRepository<WorkSchedule>, IWorkScheduleRepository
    {
        public WorkScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
