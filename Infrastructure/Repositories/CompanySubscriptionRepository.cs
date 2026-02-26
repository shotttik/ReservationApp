using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.CompanyReleated;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanySubscriptionRepository :BaseRepository<CompanySubscription>, ICompanySubscriptionRepository
    {
        public CompanySubscriptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<SubscriptionUsageDTO?> GetSubscriptionData(int companyId)
        {
            return await _dbSet
                .Where(cs => cs.CompanyId == companyId)
                .Select(cs => new SubscriptionUsageDTO
                {
                    Status = cs.Status,
                    EndDate = cs.EndDate,
                    StartDate = cs.StartDate,

                    MaxEmployees = cs.SubscriptionPlan.MaxEmployees,
                    MaxBranches = cs.SubscriptionPlan.MaxBranches,
                    MaxBookingsPerMonth = cs.SubscriptionPlan.MaxBookingsPerMonth,

                    EmployeeCount = cs.Company.UserAccounts.Count(),
                    BranchCount = cs.Company.Branches.Count(),

                    MonthlyBookingCount = cs.Company.Branches
                    .SelectMany(br => br.Bookings)
                    .Count(b =>
                        (b.Status == BookingStatus.Accepted ||
                         b.Status == BookingStatus.Completed)
                        && b.StartTime >= cs.StartDate)
                })
                .FirstOrDefaultAsync();
        }
        public async Task<CompanySubscription?> GetByCompanyId(int companyId)
        {
            return await _dbSet
                .Where(cs => cs.CompanyId == companyId)
                .FirstOrDefaultAsync();
        }

        public async Task<PagedList<CompanySubscriptionDTO>> RetrievePaged(PagedParameters parameters)
        {
            var query = _dbSet
               .AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync();

            var companies = await query
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            return new PagedList<CompanySubscriptionDTO>(companies, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
