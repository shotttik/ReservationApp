using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyFAQCategoryRepository :BaseRepository<CompanyFAQCategory>, ICompanyFAQCategoryRepository
    {
        public CompanyFAQCategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<IEnumerable<CompanyFAQCategory>> GetAll(int companyID)
        {
            return await _dbSet
                .Where(c => c.CompanyID == companyID)
                .Include(c => c.FAQs)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }
        public async Task<int> Count(int companyID)
        {
            return await _dbSet
                .Where(c => c.CompanyID == companyID)
                .CountAsync();
        }
    }
}
