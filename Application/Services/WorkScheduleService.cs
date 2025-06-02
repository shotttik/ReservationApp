using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.User;
using Domain.DTO.WorkSchedule;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class WorkScheduleService :IWorkScheduleService
    {
        private readonly IWorkScheduleRepository workScheduleRepository;
        private readonly IAuthService authService;

        public WorkScheduleService(
            IWorkScheduleRepository workScheduleRepository,
            IAuthService authService
            )
        {
            this.workScheduleRepository = workScheduleRepository;
            this.authService = authService;
        }

        public async Task<Result> WorkSchedulesCreate(WorkSchedulesCreateRequest request, bool isForEmployee)
        {
            var authUser = await authService.GetCurrentUser();

            // Validate request has all days of week
            if (request.WorkSchedules.IsNullOrEmpty() ||
                request.WorkSchedules.Select(i => i.DayOfWeek).Distinct().Count() != Enum.GetValues<DayOfWeek>().Length)
            {
                return Result.Failure(WorkScheduleResults.InvalidWorkScheduleCount);
            }

            // Validate individual schedules
            var validationResult = ValidateWorkSchedules(request.WorkSchedules);
            if (validationResult != null)
                return validationResult;

            // Check if schedules already exist
            bool existsSchedules = isForEmployee ?
                authUser.WorkSchedules.Count != 0 :
                authUser.Company!.WorkSchedules.Count != 0;

            if (existsSchedules)
            {
                return Result.Failure(WorkScheduleResults.AlreadyExists);
            }

            // Validate employee schedules are within company bounds
            if (isForEmployee && IsEmployeeOutOfBounds(request.WorkSchedules, authUser))
            {
                return Result.Failure(WorkScheduleResults.EmployeeWorkingTimesOutOfBounds);
            }

            // Create work schedules
            var workSchedules = new List<WorkSchedule>();
            foreach (var schedule in request.WorkSchedules)
            {
                var workSchedule = schedule.MapToEntity();
                workSchedule.CompanyID = authUser.Company!.ID;
                if (isForEmployee)
                {
                    workSchedule.UserID = authService.GetUserAccountID();
                }
                workSchedules.Add(workSchedule);
            }

            await workScheduleRepository.AddRange(workSchedules);
            await authService.RefreshAuthUserCache();

            return Result.Success();
        }

        public async Task<Result> WorkSchedulesUpdate(WorkSchedulesUpdateRequest request, bool isForEmployee)
        {
            var authUser = await authService.GetCurrentUser();

            // Check if schedules exist
            var existsSchedules = isForEmployee ?
                authUser.WorkSchedules.Count != 0 :
                authUser.Company!.WorkSchedules.Count != 0;

            if (!existsSchedules)
            {
                return Result.Failure(WorkScheduleResults.NotExists);
            }

            // Validate schedule IDs match existing schedules
            bool scheduleNotExists = request.WorkSchedules.Any(i =>
                isForEmployee ?
                !authUser.WorkSchedules.Select(e => e.ID).Contains(i.ID) :
                !authUser.Company!.WorkSchedules.Select(e => e.ID).Contains(i.ID)
            );

            if (scheduleNotExists)
            {
                return Result.Failure(WorkScheduleResults.Mismatch);
            }

            // Validate individual schedules
            var validationResult = ValidateWorkSchedules(request.WorkSchedules);
            if (validationResult != null)
                return validationResult;

            // Validate employee schedules are within company bounds
            if (isForEmployee && IsEmployeeOutOfBounds(request.WorkSchedules, authUser))
            {
                return Result.Failure(WorkScheduleResults.EmployeeWorkingTimesOutOfBounds);
            }

            // Update schedules
            var updatedSchedules = request.WorkSchedules.Select(schedule =>
            {
                var entity = schedule.MapToEntity();
                entity.CompanyID = authUser.Company!.ID;
                if (isForEmployee)
                {
                    entity.UserID = authService.GetUserAccountID();
                }
                return entity;
            }).ToList();

            await workScheduleRepository.UpdateRange(updatedSchedules);
            await authService.RefreshAuthUserCache();

            return Result.Success();
        }

        private static bool IsEmployeeOutOfBounds(IEnumerable<BaseWorkScheduleDTO> requestSchedules, UserAccountDTO authUser)
        {
            foreach (var employeeSchedule in requestSchedules)
            {
                var companySchedule = authUser.Company!.WorkSchedules
                    .FirstOrDefault(cs => cs.DayOfWeek == employeeSchedule.DayOfWeek);

                if (companySchedule == null)
                    return true;

                if (!companySchedule.IsWorkingDay && employeeSchedule.IsWorkingDay)
                    return true;

                if (companySchedule.IsWorkingDay && employeeSchedule.IsWorkingDay)
                {
                    if (!IsScheduleWithinBounds(employeeSchedule, companySchedule))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsScheduleWithinBounds(BaseWorkScheduleDTO employeeSchedule, BaseWorkScheduleDTO companySchedule)
        {
            // If company works 24 hours, employee can work any schedule
            if (companySchedule.Is24HourShift)
            {
                return true;
            }

            // If employee wants 24 hours but company doesn't work 24 hours
            if (employeeSchedule.Is24HourShift && !companySchedule.Is24HourShift)
            {
                return false;
            }

            // Handle overnight shifts
            if (companySchedule.IsOvernightShift)
            {
                // If company has overnight shift, employee can work within those bounds
                // This is complex logic - for now, allow if employee is also overnight or regular within bounds
                if (employeeSchedule.IsOvernightShift)
                {
                    return employeeSchedule.StartTime >= companySchedule.StartTime ||
                           employeeSchedule.EndTime <= companySchedule.EndTime;
                }
                // Regular employee schedule within overnight company schedule
                return (employeeSchedule.StartTime >= companySchedule.StartTime) ||
                       (employeeSchedule.EndTime <= companySchedule.EndTime);
            }

            // Regular schedule validation
            return employeeSchedule.StartTime >= companySchedule.StartTime &&
                   employeeSchedule.EndTime <= companySchedule.EndTime;
        }

        private static Result? ValidateWorkSchedules(IEnumerable<BaseWorkScheduleDTO> schedules)
        {
            foreach (var schedule in schedules)
            {
                // Basic working day validation
                if (!schedule.IsWorkingDay && (schedule.StartTime != null || schedule.EndTime != null))
                    return Result.Failure(WorkScheduleResults.NonWorkingDay);

                if (schedule.IsWorkingDay && (schedule.StartTime == null || schedule.EndTime == null))
                    return Result.Failure(WorkScheduleResults.NonWorkingDay);

                // Skip further validation for non-working days
                if (!schedule.IsWorkingDay)
                    continue;

                // Validate working times
                var timeValidationResult = ValidateWorkingTimes(schedule);
                if (timeValidationResult != null)
                    return timeValidationResult;

                // Validate break times
                var breakValidationResult = ValidateBreakTimes(schedule);
                if (breakValidationResult != null)
                    return breakValidationResult;

                // Validate total working hours (optional business rule)
                var hoursValidationResult = ValidateWorkingHours(schedule);
                if (hoursValidationResult != null)
                    return hoursValidationResult;
            }

            return null;
        }

        private static Result? ValidateWorkingTimes(BaseWorkScheduleDTO schedule)
        {
            if (!schedule.StartTime.HasValue || !schedule.EndTime.HasValue)
                return null;

            var startTime = schedule.StartTime.Value;
            var endTime = schedule.EndTime.Value;

            // Allow 24-hour shifts (same start and end time)
            if (startTime == endTime)
                return null;

            // Allow overnight shifts (end time before start time indicates next day)
            if (endTime < startTime)
                return null;

            // Regular shift validation (start must be before end on same day)
            if (startTime >= endTime)
                return Result.Failure(WorkScheduleResults.InvalidStartEndTime);

            return null;
        }

        private static Result? ValidateBreakTimes(BaseWorkScheduleDTO schedule)
        {
            bool hasBreakStart = schedule.BreakStartTime.HasValue;
            bool hasBreakEnd = schedule.BreakEndTime.HasValue;

            // Both break times must be provided together or not at all
            if (hasBreakStart ^ hasBreakEnd)
                return Result.Failure(WorkScheduleResults.InvalidBreakTime);

            if (hasBreakStart && hasBreakEnd)
            {
                var breakStart = schedule.BreakStartTime!.Value;
                var breakEnd = schedule.BreakEndTime!.Value;

                // Break start must be before break end
                if (breakStart >= breakEnd)
                    return Result.Failure(WorkScheduleResults.InvalidBreakTime);

                // Validate break times are within working hours
                var breakValidationResult = ValidateBreakWithinWorkingHours(schedule, breakStart, breakEnd);
                if (breakValidationResult != null)
                    return breakValidationResult;
            }

            return null;
        }

        private static Result? ValidateBreakWithinWorkingHours(BaseWorkScheduleDTO schedule, TimeOnly breakStart, TimeOnly breakEnd)
        {
            var startTime = schedule.StartTime!.Value;
            var endTime = schedule.EndTime!.Value;

            // Handle 24-hour shifts - breaks can be at any time
            if (schedule.Is24HourShift)
                return null;

            // Handle overnight shifts
            if (schedule.IsOvernightShift)
            {
                // For overnight shifts, break can be either in the first part or second part
                bool breakInFirstPart = breakStart >= startTime && breakEnd >= startTime;
                bool breakInSecondPart = breakStart <= endTime && breakEnd <= endTime;

                if (!breakInFirstPart && !breakInSecondPart)
                    return Result.Failure(WorkScheduleResults.BreakTimeOutOfRange);
            }
            else
            {
                // Regular same-day shift
                if (breakStart < startTime || breakEnd > endTime)
                    return Result.Failure(WorkScheduleResults.BreakTimeOutOfRange);
            }

            return null;
        }

        private static Result? ValidateWorkingHours(BaseWorkScheduleDTO schedule)
        {
            var workingHours = schedule.WorkingHours;

            // Optional: Add business rules for maximum working hours
            if (workingHours.TotalHours > 24)
                return Result.Failure(WorkScheduleResults.ExcessiveWorkingHours);

            // Optional: Minimum working hours validation
            if (workingHours.TotalMinutes < 30 && workingHours.TotalMinutes > 0)
                return Result.Failure(WorkScheduleResults.InsufficientWorkingHours);

            return null;
        }
    }
}

