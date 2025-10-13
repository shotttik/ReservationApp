using Application.Common.Requests.Review;
using Domain.DTO.Review;
using Domain.Entities.ReviewReleated;

namespace Application.Extensions.Mappers
{
    public static class ReviewMapper
    {
        public static Review MapToEntity(this ReviewCreateRequest request)
        {
            return new Review()
            {
                ReviewInviteId = request.InviteId,
                Cleanliness = request.Cleanliness,
                Accuracy = request.Accuracy,
                CheckIn = request.CheckIn,
                Communication = request.Communication,
                Location = request.Location,
                Value = request.Value,

                Body = request.Body,
                Locale = request.Locale
            };
        }

        public static ReviewInviteDTO MapToDTO(this ReviewInvite entity)
        {
            return new ReviewInviteDTO()
            {
                Id = entity.ID,
                BookingId = entity.BookingId,
                ClientReviewed = entity.ClientReviewed,
                OpenAt = entity.OpenAt,
                CloseAt = entity.CloseAt,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }
    }
}
