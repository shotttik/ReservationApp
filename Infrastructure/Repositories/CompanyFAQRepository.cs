using Domain.Entities.CompanyReleated;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CompanyFAQRepository :BaseRepository<CompanyFAQ>, ICompanyFAQRepository
    {
        public CompanyFAQRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<CompanyFAQ>> GetAll(int companyID, int categoryID)
        {
            return await _dbSet.Where(x => x.CategoryID == categoryID && x.Category.CompanyID == companyID).OrderBy(x => x.Order).ToListAsync();
        }

        public async Task<int> Count(int categoryID)
        {
            return await _dbSet.CountAsync(x => x.CategoryID == categoryID);
        }

        public async Task<CompanyFAQ?> GetFull(int id)
        {
            return await _dbSet.Include(x => x.Category)
                               .FirstOrDefaultAsync(x => x.ID == id);
        }
    }
}
