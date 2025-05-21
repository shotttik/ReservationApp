using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IMapper mapper;

        public CompanyRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            this.mapper = mapper;
        }

        public async Task<bool> ExistsByDetailsAsync(string IN, string name, string? email, string? phone)
        {
            return await _dbSet.AnyAsync(c =>
                c.Name == name
                || c.IN == IN ||
                (email == null || c.Email == email) ||
                (phone == null || c.Phone == phone));
        }
        public async Task<PagedList<CompanyDTO>> RetrievePaged(
            PagedParameters parameters,
            CancellationToken cancellationToken)
        {
            var query = _dbSet.AsQueryable();

            var projectedQuery = query.ProjectTo<CompanyDTO>(mapper.ConfigurationProvider);

            projectedQuery = projectedQuery.ApplyQueryParamsAsync(parameters);

            var totalCount = await projectedQuery.CountAsync();

            var companies = await projectedQuery.
                Skip((parameters.PageNumber - 1) * parameters.PageSize).
                Take(parameters.PageSize).
                ToListAsync(cancellationToken);

            return new PagedList<CompanyDTO>(companies, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
