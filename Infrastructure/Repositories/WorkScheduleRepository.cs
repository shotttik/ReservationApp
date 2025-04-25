using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    internal class WorkScheduleRepository :BaseRepository<WorkSchedule>, IWorkScheduleRepository
    {
        public WorkScheduleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task UpdateRange(IEnumerable<WorkSchedule> schedules)
        {
            _dbSet.UpdateRange(schedules);

            // tu companiis update xdeba mag shemtxveashi
            if (schedules.Any(e => e.UserID == null))
            {
                foreach (var schedule in schedules)
                {
                    if (schedule.IsWorkingDay == false)
                    {
                        var itemsToupdateWorkingDay = await _dbSet.Where(e =>
                        e.CompanyID == schedule.CompanyID
                        && e.UserID != null
                        && e.DayOfWeek == schedule.DayOfWeek
                        && e.IsWorkingDay == true
                        ).ToListAsync();
                        itemsToupdateWorkingDay.ForEach(e =>
                        {
                            e.StartTime = null;
                            e.EndTime = null;
                            e.IsWorkingDay = false;
                            e.UpdateTimestamp();
                        });
                        dbContext.UpdateRange(itemsToupdateWorkingDay);
                    }

                    var itemsToUpdateStartTime = await _dbSet.Where(e =>
                        e.CompanyID == schedule.CompanyID
                       && e.UserID != null
                       && e.DayOfWeek == schedule.DayOfWeek
                       && e.StartTime < schedule.StartTime
                        ).ToListAsync();
                    itemsToUpdateStartTime.ForEach(e =>
                    {
                        e.StartTime = schedule.StartTime;
                        e.UpdateTimestamp();
                    });
                    if (!itemsToUpdateStartTime.IsNullOrEmpty())
                    {
                        dbContext.UpdateRange(itemsToUpdateStartTime);
                    }

                    var itemsToUpdateEndTime = await _dbSet.Where(e =>
                        e.CompanyID == schedule.CompanyID
                       && e.UserID != null
                       && e.DayOfWeek == schedule.DayOfWeek
                       && e.EndTime > schedule.EndTime
                        ).ToListAsync();
                    itemsToUpdateEndTime.ForEach(e =>
                    {
                        e.EndTime = schedule.EndTime;
                        e.UpdateTimestamp();
                    });
                    if (!itemsToUpdateEndTime.IsNullOrEmpty())
                    {
                        dbContext.UpdateRange(itemsToUpdateEndTime);
                    }
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
