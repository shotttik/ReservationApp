using Domain.DTO.Branch;
using Domain.Entities.BranchReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BranchRepository :BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Branch> Add(Branch entity)
        {
            if (entity.IsMain)
            {
                await _dbSet.Where(e => e.IsMain)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.IsMain, false)
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow));
            }
            await _dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public override async Task Update(Branch entity)
        {
            if (entity.IsMain)
            {
                await _dbSet.Where(e => e.IsMain)
                    .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.IsMain, false)
                    .SetProperty(e => e.UpdatedAt, DateTime.UtcNow));
            }
            entity.UpdateTimestamp();
            _dbSet.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        // Countries da citis  wamsaghebi calke repository ar gvchirdeba,zedmetia, radgan mxolod wamoghebas vaketebt sxva arapers
        public async Task<List<StateDTO>> GetSatesByCountry(int countryID)
        {
            return await dbContext.States
                .AsNoTracking()
                .Where(s => s.CountryId == countryID)
                .Select(s => new StateDTO { Id = s.Id, Name = s.Name })
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<CityDTO>> GetCitiesByState(int stateID)
        {
            return await dbContext.Cities
                .AsNoTracking()
                .Where(c => c.StateId == stateID)
                .Select(c => new CityDTO { Id = c.Id, Name = c.Name })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<CountryDTO>> GetCountries()
        {
            return await dbContext.Countries
                .AsNoTracking()
                .Select(c => new CountryDTO { Id = c.Id, Name = c.Name })
                .Distinct()
                .ToListAsync();
        }
        public async Task<Branch?> Get(int id, int companyId)
        {
            return await _dbSet
                .Where(b => b.Id == id && b.CompanyId == companyId)
                .FirstOrDefaultAsync();
        }
        public async Task Delete(int id)
        {
            await dbContext.UserAccounts
                .Where(u => u.BranchId == id)
                .ExecuteUpdateAsync(u =>
                    u.SetProperty(x => x.BranchId, (int?)null));
            await dbContext.Bookings
                .Where(e => e.BranchId == id)
                .ExecuteDeleteAsync();
            await _dbSet.Where(e => e.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
