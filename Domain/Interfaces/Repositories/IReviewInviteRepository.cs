using Domain.Entities.ReviewReleated;

namespace Domain.Interfaces.Repositories
{
    public interface IReviewInviteRepository :IBaseRepository<ReviewInvite>
    {
        Task<ReviewInvite?> GetWithBooking(int id);
    }
}
