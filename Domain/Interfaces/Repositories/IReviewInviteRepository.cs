using Domain.Entities.ReviewReleated;
using Domain.Enums;

namespace Domain.Interfaces.Repositories
{
    public interface IReviewInviteRepository :IBaseRepository<ReviewInvite>
    {
        Task<ReviewInvite?> GetWithBooking(int id);
        Task<IEnumerable<ReviewInvite>> GetOpenReviewInvites(int userAccountId, Role role);
    }
}
