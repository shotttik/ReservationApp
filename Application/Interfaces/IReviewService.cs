using Application.Common.Requests.Review;
using Application.Common.Results;
using Domain.DTO.Review;

namespace Application.Interfaces
{
    public interface IReviewService
    {
        Task<Result> CreateInvite(int bookingId);
        Task<Result> Create(ReviewCreateRequest request);
        Task<Result<List<int>>> UploadMedia(UploadReviewMediasRequest request, CancellationToken cancellationToken);
        Task<Result<IEnumerable<ReviewInviteDTO>>> GetOpenReviewInvites();
    }
}
