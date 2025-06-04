using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.Entities;
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
        public async Task<Company> GetFullData(int id)
        {
            return await _dbSet
                .Include(c => c.Services)
                .Include(c => c.WorkSchedules)
                .FirstOrDefaultAsync(c => c.ID == id) ?? throw new KeyNotFoundException($"Company with ID {id} not found.");
        }
        public async Task<PagedList<CompanyDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken
            )
        {
            var query = _dbSet.AsQueryable();

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync();
            var companies = await query.
                Include(e => e.Services).
                Select(e => e.MapToDTO()).
                Skip((parameters.PageNumber - 1) * parameters.PageSize).
                Take(parameters.PageSize).
                ToListAsync(cancellationToken);

            return new PagedList<CompanyDTO>(companies, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
