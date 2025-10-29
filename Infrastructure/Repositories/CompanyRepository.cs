using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO.Company;
using Domain.Entities.CompanyReleated;
using Domain.Enums;
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

        public async Task<bool> ExistsByDetailsAsync(
          string IN,
          string name,
          string? email,
          string? phone,
          int? excludeId = null)
        {
            return await _dbSet.AnyAsync(c =>
                (c.Name == name || c.IN == IN ||
                 (email != null && c.Email == email) ||
                 (phone != null && c.Phone == phone))
                && (!excludeId.HasValue || c.ID != excludeId.Value));
        }
        public async Task<Company?> GetFullData(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(c => c.Services)
                .Include(c => c.Location)
                .Include(c => c.CompanyMedia)
                    .ThenInclude(cm => cm.Media)
                .Where(e => e.ID == id)
                   .FirstOrDefaultAsync();
        }
        public async Task<Company?> GetFullDataPublic(int id)
        {
            await _dbSet
            .Where(c => c.ID == id)
            .ExecuteUpdateAsync(s => s.SetProperty(c => c.Viewed, c => c.Viewed + 1));

            var company = await _dbSet
                .AsNoTracking()
                .Include(c => c.Services
                    .Where(s => s.ActiveStatus == ActiveStatus.Active))
                .Include(c => c.Location)
                .Include(c => c.CompanyMedia)
                    .ThenInclude(cm => cm.Media)
                .Where(e => e.ID == id && e.ActiveStatus == ActiveStatus.Active)
                   .FirstOrDefaultAsync();

            return company;
        }
        public async Task<PagedList<CompanyDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken,
            bool forPublic // only active companies
        )
        {
            var query = _dbSet
                .Include(e => e.Services)
                .Include(e => e.Location)
                .AsQueryable();

            if (forPublic)
            {
                query = query.Where(c => c.ActiveStatus == ActiveStatus.Active);
            }

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync(cancellationToken);

            var companies = await query
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<CompanyDTO>(companies, parameters.PageNumber, parameters.PageSize, totalCount);
        }

        public async Task<Company?> GetWithLocation(int id)
        {
            return await _dbSet.Where(e => e.ID == id)
                .Include(e => e.Location)
                .FirstOrDefaultAsync();
        }
        public async Task<Company?> GetWithMedia(int id)
        {
            return await _dbSet.Where(e => e.ID == id)
                .Include(e => e.CompanyMedia)
                    .ThenInclude(e => e.Media)
                .FirstOrDefaultAsync();
        }
    }
}
