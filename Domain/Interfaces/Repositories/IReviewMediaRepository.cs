using Domain.Entities.ReviewReleated;

namespace Domain.Interfaces.Repositories
{
    public interface IReviewMediaRepository
    {
        Task AddRange(IEnumerable<ReviewMedia> reviewMedias);
    }
}
