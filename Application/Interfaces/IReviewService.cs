using Application.Common.Requests.Review;
using Application.Common.Results;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<Result> CreateInvite(int bookingId);
        Task<Result> Create(ReviewCreateRequest request);
    }
}
