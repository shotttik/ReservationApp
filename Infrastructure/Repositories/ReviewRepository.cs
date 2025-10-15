using Application.Extensions.Mappers;
using Domain.Abstractions;
using Domain.DTO.Review;
using Domain.Entities.ReviewReleated;
using Domain.Interfaces.Repositories;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ReviewRepository :BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext dbContext) : base(dbContext) { }
        public async Task<PagedList<ReviewDTO>> RetrievePaged(PagedParameters parameters, bool forPublic, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsQueryable();
            query = query.ApplyQueryParamsAsync(parameters);

            if (forPublic == true)
            {
                query = query.Where(e => e.Status == Domain.Enums.ReviewStatus.Published);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .Select(e => e.MapToDTO())
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<ReviewDTO>(reviews, parameters.PageNumber, parameters.PageSize, totalCount);
        }
    }
}
