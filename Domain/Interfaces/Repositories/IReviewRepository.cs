using Domain.Abstractions;
using Domain.DTO.Review;
using Domain.Entities.ReviewReleated;

namespace Domain.Interfaces.Repositories
{
    public interface IReviewRepository :IBaseRepository<Review>
    {
        Task<PagedList<ReviewDTO>> RetrievePaged(PagedParameters parameters, bool forPublic, CancellationToken cancellationToken);
    }
}
