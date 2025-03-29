using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class CompanyRepository :ICompanyRepository
    {
        private readonly ApplicationDbContext context;

        public CompanyRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<int> Add(Company company)
        {
            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            return company.ID;
        }
    }
}
