using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWorkScheduleRepository :IBaseRepository<WorkSchedule>
    {
        //Task UpdateRange(IEnumerable<WorkSchedule> schedules);
    }
}
