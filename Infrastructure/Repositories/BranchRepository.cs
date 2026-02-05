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
        // Countries da citis  wamsaghebi calke repository ar gvchirdeba,zedmetia, radgan mxolod wamoghebas vaketebt sxva arapers
        public async Task<List<StateDTO>> GetSatesByCountry(int countryID)
        {
            return await dbContext.States
                .AsNoTracking()
                .Where(s => s.CountryId == countryID)
                .Select(s => new StateDTO { ID = s.ID, Name = s.Name })
                .Distinct()
                .ToListAsync();
        }
        public async Task<List<CityDTO>> GetCitiesByState(int stateID)
        {
            return await dbContext.Cities
                .AsNoTracking()
                .Where(c => c.StateId == stateID)
                .Select(c => new CityDTO { ID = c.ID, Name = c.Name })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<CountryDTO>> GetCountries()
        {
            return await dbContext.Countries
                .AsNoTracking()
                .Select(c => new CountryDTO { ID = c.ID, Name = c.Name })
                .Distinct()
                .ToListAsync();
        }
    }
}
