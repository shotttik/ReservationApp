using Application.Common.Requests.WorkSchedule;
using Application.Common.Results;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.DTO;
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
        private readonly ICacheService cacheService;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IUserLoginDataRepository userLoginDataRepository;

        public WorkScheduleService(
            IWorkScheduleRepository workScheduleRepository,
            IAuthService authService,
            ICacheService cacheService,
            IUserAccountRepository userAccountRepository,
            IUserLoginDataRepository userLoginDataRepository)
        {
            this.workScheduleRepository = workScheduleRepository;
            this.authService = authService;
            this.cacheService = cacheService;
            this.userAccountRepository = userAccountRepository;
            this.userLoginDataRepository = userLoginDataRepository;
        }
        public async Task<Result> WorkSchedulesCreate(WorkSchedulesCreateRequest request, bool isForEmployee)
        {
            var AuthUser = await authService.GetCurrentUser();

            // mxolod company unda ikos mititebuli roca companys samushao cxrilis damatebaa
            if (request.WorkSchedules.IsNullOrEmpty()
                || request.WorkSchedules.Select(i => i.DayOfWeek).Distinct().Count() != Enum.GetValues<DayOfWeek>().Length)
            {
                return Result.Failure(WorkScheduleResults.InvalidWorkScheduleCount);
            }
            var validationResult = ValidateWorkSchedules(request.WorkSchedules);
            if (validationResult != null)
                return validationResult;


            bool existsSchedules = isForEmployee ? AuthUser.WorkSchedules.Count != 0 : AuthUser.Company!.WorkSchedules.Count != 0;
            if (existsSchedules)
            {
                return Result.Failure(WorkScheduleResults.AlreadyExists);
            }

            if (isForEmployee && IsEmployeeOutOfBounds(request.WorkSchedules, AuthUser))
            {
                return Result.Failure(WorkScheduleResults.EmployeeWorkingTimesOutOfBounds);
            }

            var workSchedules = new List<WorkSchedule>();
            foreach (var schedule in request.WorkSchedules)
            {
                var workSchedule = schedule.MapToEntity();
                workSchedule.CompanyID = AuthUser.Company!.ID;
                if (isForEmployee)
                {
                    workSchedule.UserID = AuthUser.ID;
                }
                workSchedules.Add(workSchedule);
            }

            await workScheduleRepository.AddRange(workSchedules);
            await authService.RefreshAuthUserCache();

            return Result.Success();
        }

        public async Task<Result> WorkSchedulesUpdate(WorkSchedulesUpdateRequest request, bool isForEmployee)
        {
            var AuthUser = await authService.GetCurrentUser();

            var existsSchedules = AuthUser.Company!.WorkSchedules.Count != 0;
            if (!existsSchedules)
            {
                return Result.Failure(WorkScheduleResults.NotExists);
            }
            bool scheduleNotExists =
                request.WorkSchedules.Any(i =>
                    isForEmployee ?
                    !AuthUser.WorkSchedules.Select(e => e.ID).Contains(i.ID) :
                    !AuthUser.Company.WorkSchedules.Select(e => e.ID).Contains(i.ID)
                );
            if (scheduleNotExists)
            {
                return Result.Failure(WorkScheduleResults.Mismatch);
            }

            var validationResult = ValidateWorkSchedules(request.WorkSchedules);
            if (validationResult != null)
                return validationResult;

            if (isForEmployee && IsEmployeeOutOfBounds(request.WorkSchedules, AuthUser))
            {
                return Result.Failure(WorkScheduleResults.EmployeeWorkingTimesOutOfBounds);
            }

            var updatedSchedules = request.WorkSchedules.Select(schedule =>
            {
                var entity = schedule.MapToEntity();
                entity.CompanyID = AuthUser.Company.ID;
                if (isForEmployee)
                {
                    entity.UserID = AuthUser.ID;
                }
                return entity;
            }
            ).ToList();

            await workScheduleRepository.UpdateRange(updatedSchedules);

            await authService.RefreshAuthUserCache();

            return Result.Success();
        }
        private bool IsEmployeeOutOfBounds(IEnumerable<BaseWorkScheduleDTO> requestSchedules, UserAccountDTO authUser)
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
                    if (employeeSchedule.StartTime < companySchedule.StartTime ||
                        employeeSchedule.EndTime > companySchedule.EndTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private Result? ValidateWorkSchedules(IEnumerable<BaseWorkScheduleDTO> schedules)
        {
            if (schedules.Any(i => i.StartTime >= i.EndTime))
                return Result.Failure(WorkScheduleResults.InvalidStartEndTime);

            if (schedules.Any(i => !i.IsWorkingDay && (i.StartTime != null || i.EndTime != null)))
                return Result.Failure(WorkScheduleResults.NonWorkingDay);

            if (schedules.Any(i => i.IsWorkingDay && (i.StartTime == null || i.EndTime == null)))
                return Result.Failure(WorkScheduleResults.NonWorkingDay);

            return null;
        }
    }
}
