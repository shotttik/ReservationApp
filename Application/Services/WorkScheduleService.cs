using Application.Common.ResultsErrors;
using Application.Common.ResultsErrors.WorkSchedule;
using Application.DTOs.WorkSchedule;
using Application.Extensions.Mappers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Shared.Utilities;
using static Application.Common.ResultsErrors.WorkSchedule.WorkSchedulesErrors;

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
        public async Task<Result> AddCompanyWorkSchedules(WorkSchedulesRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();

            // mxolod company unda ikos mititebuli roca companys samushao cxrilis damatebaa
            if (request.WorkSchedules.Any(i => i.UserID.HasValue))
            {
                return Result.Failure(AddCompanyWorkSchedulesErrors.UserMentioned);
            }
            if (request.WorkSchedules.IsNullOrEmpty()
                || request.WorkSchedules.Select(i => i.DayOfWeek).Distinct().Count() != 7)
            {
                return Result.Failure(AddCompanyWorkSchedulesErrors.InvalidWorkScheduleCount);
            }
            var validationResult = ValidateWorkSchedulesRequest(request);
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
                workSchedules.Add(workSchedule);
            }

            await workScheduleRepository.AddRange(workSchedules);
            var userAccount = await userAccountRepository.GetAuthorizationData(AuthUser.ID);
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(AuthUser.ID), userAccount!.MapToAuthorizationData());

            return Result.Success();
        }

        public async Task<Result> UpdateCompanyWorkSchedules(WorkSchedulesRequest request)
        {
            var AuthUser = await authService.GetCurrentUser();

            if (request.WorkSchedules.Any(i => i.UserID.HasValue))
            {
                return Result.Failure(UpdateCompanyWorkSchedulesErrors.UserMentioned);
            }

            var existsSchedules = AuthUser.Company!.WorkSchedules.Count != 0;
            if (!existsSchedules)
            {
                return Result.Failure(UpdateCompanyWorkSchedulesErrors.NotExists);
            }
            var scheduleNotExistsInCompany = request.WorkSchedules.Any(i => !AuthUser.Company.WorkSchedules.Select(e => e.ID).Contains(i.ID)
                || i.CompanyID != AuthUser.Company.ID);
            if (scheduleNotExistsInCompany)
            {
                return Result.Failure(UpdateCompanyWorkSchedulesErrors.Mismatch);
            }

            var validationResult = ValidateWorkSchedulesRequest(request);
            if (validationResult != null)
                return validationResult;

            var updatedSchedules = request.WorkSchedules.Select(schedule => schedule.MapToEntity()).ToList();

            await workScheduleRepository.UpdateRange(updatedSchedules);

            var userAccount = await userAccountRepository.GetAuthorizationData(AuthUser.ID);
            await cacheService.SetAsync(CacheUtils.AuthorizationCacheKey(AuthUser.ID), userAccount!.MapToAuthorizationData());

            return Result.Success();
        }

        private Result? ValidateWorkSchedulesRequest(WorkSchedulesRequest request)
        {
            if (request.WorkSchedules.Any(i => i.StartTime >= i.EndTime))
            {
                return Result.Failure(WorkSchedulesErrors.InvalidStartEndTime);
            }

            if (request.WorkSchedules.Any(i =>
                i.IsWorkingDay == false && (i.StartTime != null || i.EndTime != null))
                )
            {
                return Result.Failure(WorkSchedulesErrors.NonWorkingDay);
            }
            if (request.WorkSchedules.Any(i =>
                i.IsWorkingDay == true && (i.StartTime == null || i.EndTime == null))
                )
            {
                return Result.Failure(WorkSchedulesErrors.NonWorkingDay);
            }

            return null;
        }
    }
}
