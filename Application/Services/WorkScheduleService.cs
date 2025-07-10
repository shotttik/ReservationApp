using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO.WorkSchedule;
using Domain.Entities.Common;
using Domain.Interfaces.Repositories;

namespace Application.Services
{
    public class WorkScheduleService :IWorkScheduleService
    {
        private readonly IWorkScheduleRepository workScheduleRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IAccessGuard accessGuard;
        private readonly IAuthService authService;

        public WorkScheduleService(
            IWorkScheduleRepository workScheduleRepository,
            IUserAccountRepository userAccountRepository,
            IAccessGuard accessGuard,
            IAuthService authService
            )
        {
            this.workScheduleRepository = workScheduleRepository;
            this.userAccountRepository = userAccountRepository;
            this.accessGuard = accessGuard;
            this.authService = authService;
        }

        public async Task<Result> Create(WorkScheduleCreateRequest request)
        {
            if (request.StartTime >= request.EndTime)
                return Result.Failure(WorkScheduleResults.InvalidTimeRange);

            var userAccount = await userAccountRepository.GetByUserLoginDataIDWithWorkSchedules(request.UserID);

            if (userAccount == null || userAccount.CompanyID == null)
            {
                return Result.Failure(GenericResults.DontExists);
            }
            var accessError = await accessGuard.EnsureAccessToCompanyMember((int)userAccount.CompanyID, request.UserID);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }
            // Fetch existing schedules for that day
            var existingSchedules = userAccount.WorkSchedules.Where(e => e.DayOfWeek == request.DayOfWeek);
            foreach (var schedule in existingSchedules)
            {
                if (IsOverlapping(request.StartTime, request.EndTime, schedule.StartTime!.Value, schedule.EndTime!.Value))
                {
                    return Result.Failure(WorkScheduleResults.OverlappingSchedule);
                }
            }

            var workSchedule = new WorkSchedule
            {
                UserAccountID = userAccount.ID,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            await workScheduleRepository.Add(workSchedule);
            await authService.RefreshUserCache(request.UserID);

            return Result.Success();
        }

        public async Task<Result> Update(
            WorkScheduleUpdateRequest request)
        {
            var schedule = await workScheduleRepository.Get(request.ID);

            if (schedule == null)
                return Result.Failure(WorkScheduleResults.DoesntExists);

            if (request.StartTime >= request.EndTime)
                return Result.Failure(WorkScheduleResults.InvalidTimeRange);

            var userAccount = await userAccountRepository.GetByUserLoginDataIDWithWorkSchedules(request.UserID);

            if (userAccount == null || userAccount.CompanyID == null)
            {
                return Result.Failure(GenericResults.DontExists);
            }
            if (userAccount.ID != schedule.UserAccountID)
            {
                return Result.Failure(GenericResults.Forbidden);
            }

            var accessError = await accessGuard.EnsureAccessToCompanyMember((int)userAccount.CompanyID, request.UserID);
            if (accessError != Error.None)
            {
                return Result.Failure(accessError);
            }

            // Check against other schedules of same user & day
            var otherSchedules = userAccount.WorkSchedules
                .Where(e => e.ID != request.ID && e.DayOfWeek == schedule.DayOfWeek)
                .ToList();
            foreach (var s in otherSchedules)
            {
                if (IsOverlapping(request.StartTime, request.EndTime, s.StartTime!.Value, s.EndTime!.Value))
                {
                    return Result.Failure(WorkScheduleResults.OverlappingSchedule);
                }
            }

            schedule.StartTime = request.StartTime;
            schedule.EndTime = request.EndTime;

            await workScheduleRepository.Update(schedule);
            await authService.RefreshUserCache(request.UserID);

            return Result.Success(WorkScheduleResults.Updated); ;
        }

        public async Task<Result> Delete(int id)
        {

            var schedule = await workScheduleRepository.Get(id);

            if (schedule == null)
                return Result.Failure(WorkScheduleResults.DoesntExists);

            var userAccount = await userAccountRepository.Get(schedule.UserAccountID);
            if (userAccount == null)
                return Result.Failure(GenericResults.Forbidden);

            var accessError = await accessGuard.EnsureAccessToCompanyMember((int)userAccount.CompanyID!, userAccount.UserLoginDataID);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            await workScheduleRepository.Delete(schedule);

            return Result.Success(WorkScheduleResults.Deleted);
        }

        public async Task<Result<List<WorkScheduleDTO>>> GetAllForUser(int userId)
        {
            var schedules = await workScheduleRepository.GetAllForUser(userId);

            var schedulesDTOs = new List<WorkScheduleDTO>();

            foreach (var item in schedules)
            {
                schedulesDTOs.Add(item.MapToDTO());
            }

            return Result.Success(schedulesDTOs);
        }
        private bool IsOverlapping(TimeOnly newStart, TimeOnly newEnd, TimeOnly existingStart, TimeOnly existingEnd)
        {
            return newStart < existingEnd && newEnd > existingStart;
        }
    }
}

