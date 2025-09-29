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
        private readonly IWorkScheduleExceptionRepository workScheduleExceptionRepository;

        public WorkScheduleService(
            IWorkScheduleRepository workScheduleRepository,
            IUserAccountRepository userAccountRepository,
            IAccessGuard accessGuard,
            IAuthService authService,
            IWorkScheduleExceptionRepository workScheduleExceptionRepository
            )
        {
            this.workScheduleRepository = workScheduleRepository;
            this.userAccountRepository = userAccountRepository;
            this.accessGuard = accessGuard;
            this.authService = authService;
            this.workScheduleExceptionRepository = workScheduleExceptionRepository;
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
            var accessError = await accessGuard.EnsureAccessToCompanyEmployee((int)userAccount.CompanyID, request.UserID);
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

            var accessError = await accessGuard.EnsureAccessToCompanyEmployee((int)userAccount.CompanyID, request.UserID);
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

            var accessError = await accessGuard.EnsureAccessToCompanyEmployee((int)userAccount.CompanyID!, userAccount.UserLoginDataID);
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
        public async Task<Result> CreateException(WorkScheduleExceptionCreateRequest request)
        {
            if (request.StartDate > request.EndDate)
                return Result.Failure(WorkScheduleResults.InvalidDateRange);

            var userAccount = await userAccountRepository.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserID);

            if (userAccount == null || userAccount.CompanyID == null)
                return Result.Failure(GenericResults.DontExists);

            var accessError = await accessGuard.EnsureAccessToCompanyEmployee((int)userAccount.CompanyID, request.UserID);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            bool hasOverlap = userAccount.WorkScheduleExceptions.Any(e =>
                 (request.StartDate <= e.EndDate && request.EndDate >= e.StartDate));

            if (hasOverlap)
            {
                return Result.Failure(WorkScheduleResults.OverlappingException);
            }

            var workScheduleException = request.MapToEntity();
            workScheduleException.UserAccountID = userAccount.ID;

            await workScheduleExceptionRepository.Add(workScheduleException);

            return Result.Success();
        }

        public async Task<Result> UpdateException(WorkScheduleExceptionUpdateRequest request)
        {
            if (request.StartDate > request.EndDate)
                return Result.Failure(WorkScheduleResults.InvalidDateRange);

            var workScheduleException = await workScheduleExceptionRepository.Get(request.ID);

            if (workScheduleException == null)
                return Result.Failure(WorkScheduleResults.DoesntExists);

            var userAccount = await userAccountRepository.GetByUserLoginDataIDWithWorkScheduleExceptions(request.UserID);

            if (userAccount == null || userAccount.CompanyID == null)
                return Result.Failure(GenericResults.DontExists);

            if (workScheduleException.UserAccountID != userAccount.ID)
                return Result.Failure(GenericResults.Forbidden);

            var accessError = await accessGuard.EnsureAccessToCompanyEmployee((int)userAccount.CompanyID, request.UserID);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            // Check for overlapping exceptions (excluding self)
            bool hasOverlap = userAccount.WorkScheduleExceptions
                .Where(e => e.ID != request.ID)
                .Any(e => request.StartDate <= e.EndDate && request.EndDate >= e.StartDate);

            if (hasOverlap)
                return Result.Failure(WorkScheduleResults.OverlappingException);

            request.MapToEntity(workScheduleException);

            await workScheduleExceptionRepository.Update(workScheduleException);

            return Result.Success(WorkScheduleResults.Updated);
        }

        public async Task<Result> DeleteException(int id)
        {
            var workScheduleException = await workScheduleExceptionRepository.Get(id);

            if (workScheduleException == null)
                return Result.Failure(WorkScheduleResults.DoesntExists);

            var userAccount = await userAccountRepository.Get(workScheduleException.UserAccountID);

            if (userAccount == null || userAccount.CompanyID == null)
                return Result.Failure(GenericResults.DontExists);

            var accessError = await accessGuard.EnsureAccessToCompanyEmployee((int)userAccount.CompanyID, userAccount.UserLoginDataID);
            if (accessError != Error.None)
                return Result.Failure(accessError);

            await workScheduleExceptionRepository.Delete(workScheduleException);

            return Result.Success(WorkScheduleResults.Deleted);
        }

        public async Task<Result<List<WorkScheduleExceptionDTO>>> GetAllExceptionForUser(int userId)
        {
            var schedules = await workScheduleExceptionRepository.GetAllForUser(userId);

            var schedulesDTOs = new List<WorkScheduleExceptionDTO>();

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

