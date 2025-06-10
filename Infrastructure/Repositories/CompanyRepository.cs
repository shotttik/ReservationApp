using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyRepository :BaseRepository<Company>, ICompanyRepository
    {

        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsByDetailsAsync(string IN, string name, string? email, string? phone)
        {
            return await _dbSet.AnyAsync(c =>
                c.Name == name
                || c.IN == IN ||
                (email == null || c.Email == email) ||
                (phone == null || c.Phone == phone));
        }
        public async Task<Company?> GetFullData(int id)
        {
            return await _dbSet
                .Include(c => c.Services)
                .Include(c => c.WorkSchedules)
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.ID == id);
        }
        public async Task<PagedList<CompanyDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken,
            bool forPublic // only active companies
            )
        {

            var query = _dbSet.AsQueryable();
            query = query.ApplyQueryParamsAsync(parameters);

            if (!forPublic)
            {
                query = query.Where(c => c.IsActive);
            }

            var totalCount = await query.CountAsync();
            var companies = await query.
                Include(e => e.Services).
                Include(e => e.Location).
                Select(e => e.MapToDTO()).
                Skip((parameters.PageNumber - 1) * parameters.PageSize).
                Take(parameters.PageSize).
                ToListAsync(cancellationToken);

            return new PagedList<CompanyDTO>(companies, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
