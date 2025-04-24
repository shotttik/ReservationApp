using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.WorkSchedule;
using Application.DTOs.WorkSchedule;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Shared.Utilities;

namespace Application.Services
{
    public class WorkScheduleService :IWorkScheduleService
    {
        private readonly IWorkScheduleRepository workScheduleRepository;
        private readonly IAuthService authService;
        private readonly ICacheService cacheService;
        private readonly IUserAccountRepository userAccountRepository;

        public WorkScheduleService(
            IWorkScheduleRepository workScheduleRepository,
            IAuthService authService,
            ICacheService cacheService,
            IUserAccountRepository userAccountRepository)
        {
            this.workScheduleRepository = workScheduleRepository;
            this.authService = authService;
            this.cacheService = cacheService;
            this.userAccountRepository = userAccountRepository;
        }
        public async Task<Result> AddCompanyWorkSchedules(AddWorkSchedulesRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();

            // mxolod company unda ikos mititebuli roca companys samushao cxrilis damatebaa
            if (request.WorkSchedules.IsNullOrEmpty()
                || request.WorkSchedules.Select(i => i.DayOfWeek).Distinct().Count() != Enum.GetValues<DayOfWeek>().Length)
            {
                return Result.Failure(AddCompanyWorkSchedulesErrors.InvalidWorkScheduleCount);
            }
            var validationResult = ValidateWorkSchedules(request.WorkSchedules);
            if (validationResult != null)
                return validationResult;

            var existsSchedules = AuthUser.Company!.WorkSchedules.Count != 0;
            if (existsSchedules)
            {
                return Result.Failure(AddCompanyWorkSchedulesErrors.AlreadyExists);
            }
            var workSchedules = new List<WorkSchedule>();
            foreach (var schedule in request.WorkSchedules)
            {
                var workSchedule = schedule.MapToEntity();
                workSchedule.CompanyID = AuthUser.Company.ID;
                workSchedules.Add(workSchedule);
            }

            await workScheduleRepository.AddRange(workSchedules);
            var userAccount = await userAccountRepository.GetAuthorizationData(AuthUser.ID);
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(AuthUser.ID), userAccount!.MapToAuthorizationData());

            return Result.Success();
        }

        public async Task<Result> UpdateCompanyWorkSchedules(UpdateWorkSchedulesRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();

            var existsSchedules = AuthUser.Company!.WorkSchedules.Count != 0;
            if (!existsSchedules)
            {
                return Result.Failure(UpdateCompanyWorkSchedulesErrors.NotExists);
            }
            var scheduleNotExistsInCompany = request.WorkSchedules.Any(i => !AuthUser.Company.WorkSchedules.Select(e => e.ID).Contains(i.ID));
            if (scheduleNotExistsInCompany)
            {
                return Result.Failure(UpdateCompanyWorkSchedulesErrors.Mismatch);
            }

            var validationResult = ValidateWorkSchedules(request.WorkSchedules);
            if (validationResult != null)
                return validationResult;

            var updatedSchedules = request.WorkSchedules.Select(schedule =>
            {
                var entity = schedule.MapToEntity();
                entity.CompanyID = AuthUser.Company.ID;
                return entity;
            }
            ).ToList();

            await workScheduleRepository.UpdateRange(updatedSchedules);

            var userAccount = await userAccountRepository.GetAuthorizationData(AuthUser.ID);
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(AuthUser.ID), userAccount!.MapToAuthorizationData());

            return Result.Success();
        }

        private Result? ValidateWorkSchedules(IEnumerable<BaseWorkScheduleDTO> schedules)
        {
            if (schedules.Any(i => i.StartTime >= i.EndTime))
                return Result.Failure(WorkSchedulesErrors.InvalidStartEndTime);

            if (schedules.Any(i => !i.IsWorkingDay && (i.StartTime != null || i.EndTime != null)))
                return Result.Failure(WorkSchedulesErrors.NonWorkingDay);

            if (schedules.Any(i => i.IsWorkingDay && (i.StartTime == null || i.EndTime == null)))
                return Result.Failure(WorkSchedulesErrors.NonWorkingDay);

            return null;
        }
    }
}
