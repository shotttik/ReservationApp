using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
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

            // Process company schedule updates (when UserID is null)
            var companySchedules = schedules.Where(e => e.UserID == null);
            if (companySchedules.Any())
            {
                await UpdateEmployeeSchedulesBasedOnCompanyChanges(companySchedules);
            }

            await dbContext.SaveChangesAsync();
        }

        private async Task UpdateEmployeeSchedulesBasedOnCompanyChanges(IEnumerable<WorkSchedule> companySchedules)
        {
            foreach (var companySchedule in companySchedules)
            {
                await ProcessCompanyScheduleUpdate(companySchedule);
            }
        }

        private async Task ProcessCompanyScheduleUpdate(WorkSchedule companySchedule)
        {
            // Get all employee schedules for this company and day
            var employeeSchedules = await _dbSet.Where(e =>
                e.CompanyID == companySchedule.CompanyID
                && e.UserID != null
                && e.DayOfWeek == companySchedule.DayOfWeek
            ).ToListAsync();

            if (employeeSchedules.IsNullOrEmpty())
                return;

            // Handle non-working day updates
            if (!companySchedule.IsWorkingDay)
            {
                UpdateEmployeesToNonWorkingDay(employeeSchedules);
                return;
            }

            // Handle working day updates
            UpdateEmployeeWorkingSchedules(employeeSchedules, companySchedule);
        }

        private void UpdateEmployeesToNonWorkingDay(List<WorkSchedule> employeeSchedules)
        {
            var schedulesToUpdate = employeeSchedules.Where(e => e.IsWorkingDay).ToList();

            schedulesToUpdate.ForEach(e =>
            {
                e.StartTime = null;
                e.EndTime = null;
                e.BreakStartTime = null;
                e.BreakEndTime = null;
                e.IsWorkingDay = false;
                e.UpdateTimestamp();
            });

            if (schedulesToUpdate.Count != 0)
            {
                dbContext.UpdateRange(schedulesToUpdate);
            }
        }

        private void UpdateEmployeeWorkingSchedules(List<WorkSchedule> employeeSchedules, WorkSchedule companySchedule)
        {
            var schedulesToUpdate = new List<WorkSchedule>();

            foreach (var employeeSchedule in employeeSchedules.Where(e => e.IsWorkingDay))
            {
                bool needsUpdate = false;

                // If company is 24 hours, employee schedules don't need adjustment
                if (companySchedule.Is24HourShift)
                {
                    continue;
                }

                // If employee has 24-hour shift but company doesn't, adjust employee to company schedule
                if (employeeSchedule.Is24HourShift && !companySchedule.Is24HourShift)
                {
                    employeeSchedule.StartTime = companySchedule.StartTime;
                    employeeSchedule.EndTime = companySchedule.EndTime;
                    employeeSchedule.BreakStartTime = null;
                    employeeSchedule.BreakEndTime = null;
                    needsUpdate = true;
                }
                else if (!employeeSchedule.Is24HourShift && !companySchedule.Is24HourShift)
                {
                    // Handle regular schedule adjustments
                    if (ShouldUpdateStartTime(employeeSchedule, companySchedule))
                    {
                        employeeSchedule.StartTime = companySchedule.StartTime;
                        needsUpdate = true;
                    }

                    if (ShouldUpdateEndTime(employeeSchedule, companySchedule))
                    {
                        employeeSchedule.EndTime = companySchedule.EndTime;
                        needsUpdate = true;
                    }

                    // Validate and adjust break times if schedule was modified
                    if (needsUpdate && HasBreakTimes(employeeSchedule))
                    {
                        if (!IsBreakTimeValid(employeeSchedule))
                        {
                            employeeSchedule.BreakStartTime = null;
                            employeeSchedule.BreakEndTime = null;
                        }
                    }
                }

                if (needsUpdate)
                {
                    employeeSchedule.UpdateTimestamp();
                    schedulesToUpdate.Add(employeeSchedule);
                }
            }

            if (schedulesToUpdate.Count != 0)
            {
                dbContext.UpdateRange(schedulesToUpdate);
            }
        }

        private static bool ShouldUpdateStartTime(WorkSchedule employeeSchedule, WorkSchedule companySchedule)
        {
            if (!employeeSchedule.StartTime.HasValue || !companySchedule.StartTime.HasValue)
                return false;

            // Handle overnight shifts comparison
            if (employeeSchedule.IsOvernightShift && companySchedule.IsOvernightShift)
            {
                return employeeSchedule.StartTime < companySchedule.StartTime;
            }

            if (employeeSchedule.IsOvernightShift && !companySchedule.IsOvernightShift)
            {
                // Employee has overnight, company has regular - need to adjust
                return true;
            }

            if (!employeeSchedule.IsOvernightShift && companySchedule.IsOvernightShift)
            {
                // Employee has regular, company has overnight - check if employee starts before company
                return employeeSchedule.StartTime < companySchedule.StartTime;
            }

            // Both are regular shifts
            return employeeSchedule.StartTime < companySchedule.StartTime;
        }

        private static bool ShouldUpdateEndTime(WorkSchedule employeeSchedule, WorkSchedule companySchedule)
        {
            if (!employeeSchedule.EndTime.HasValue || !companySchedule.EndTime.HasValue)
                return false;

            // Handle overnight shifts comparison
            if (employeeSchedule.IsOvernightShift && companySchedule.IsOvernightShift)
            {
                return employeeSchedule.EndTime > companySchedule.EndTime;
            }

            if (employeeSchedule.IsOvernightShift && !companySchedule.IsOvernightShift)
            {
                // Employee has overnight, company has regular - need to adjust
                return true;
            }

            if (!employeeSchedule.IsOvernightShift && companySchedule.IsOvernightShift)
            {
                // Employee has regular, company has overnight - check if employee ends after company
                return employeeSchedule.EndTime > companySchedule.EndTime;
            }

            // Both are regular shifts
            return employeeSchedule.EndTime > companySchedule.EndTime;
        }

        private static bool HasBreakTimes(WorkSchedule schedule)
        {
            return schedule.BreakStartTime.HasValue || schedule.BreakEndTime.HasValue;
        }

        private static bool IsBreakTimeValid(WorkSchedule schedule)
        {
            if (!schedule.BreakStartTime.HasValue || !schedule.BreakEndTime.HasValue)
                return true; // No break times to validate

            var breakStart = schedule.BreakStartTime.Value;
            var breakEnd = schedule.BreakEndTime.Value;

            // Break start must be before break end
            if (breakStart >= breakEnd)
                return false;

            // Handle 24-hour shifts - breaks can be at any time
            if (schedule.Is24HourShift)
                return true;

            var startTime = schedule.StartTime!.Value;
            var endTime = schedule.EndTime!.Value;

            // Handle overnight shifts
            if (schedule.IsOvernightShift)
            {
                // Break can be either in the first part (start to midnight) or second part (midnight to end)
                bool breakInFirstPart = breakStart >= startTime && breakEnd >= startTime;
                bool breakInSecondPart = breakStart <= endTime && breakEnd <= endTime;
                return breakInFirstPart || breakInSecondPart;
            }

            // Regular shift - break must be within working hours
            return breakStart >= startTime && breakEnd <= endTime;
        }
    }
}
