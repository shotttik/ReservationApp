using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO;
using Domain.Entities.Common;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PromoCodeRepository :BaseRepository<PromoCode>, IPromoCodeRepository
    {
        public PromoCodeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public void UpdateWithoutSave(PromoCode entity)
        {
            entity.UpdateTimestamp();
            _dbSet.Update(entity);
        }

        public async Task<PromoCode?> Get(string code, int companyId)
        {
            return await _dbSet.Where(e => e.Code == code
            && e.CompanyId == companyId).FirstOrDefaultAsync();
        }
        public async Task<PromoCode?> Get(int id, int companyId)
        {
            return await _dbSet.Where(e => e.Id == id
            && e.CompanyId == companyId).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistsByCode(string code)
        {
            return await _dbSet.AnyAsync(e => e.Code == code);
        }
        public async Task<bool> ExistsByCode(string code, int id)
        {
            return await _dbSet.AnyAsync(e => e.Code == code && e.Id != id);
        }

        public async Task<PagedList<PromoCodeDTO>> RetrievePaged(PagedParameters parameters, CancellationToken cancellationToken, int companyID)
        {
            var query = _dbSet.Where(e => e.CompanyId == companyID);

            query = query.ApplyQueryParamsAsync(parameters);

            var totalCount = await query.CountAsync(cancellationToken);

            var promos = await query
                .OrderBy(e=> e.Id)
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<PromoCodeDTO>(promos, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
