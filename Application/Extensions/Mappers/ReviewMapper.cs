using Application.Common.Requests.Review;
using Domain.DTO.Review;
using Domain.Entities.ReviewReleated;
using Domain.Enums;

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

        public static ReviewDTO MapToDTO(this Review entity)
        {
            return new ReviewDTO()
            {
                Id = entity.Id,
                Status = entity.Status,
                Overall = entity.Overall,
                Cleanliness = entity.Cleanliness,
                Accuracy = entity.Accuracy,
                CheckIn = entity.CheckIn,
                Communication = entity.Communication,
                Location = entity.Location,
                Value = entity.Value,
                Body = entity.Body,
                Locale = entity.Locale,
                PublishedAt = entity.PublishedAt,
                ReviewInviteId = entity.ReviewInviteId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Media = entity.Media.Select(e => new ReviewMediaDTO
                {
                    Id = e.Media.Id,
                    ImageUrlWebp = e.Media.RemoteUrl,
                    ImageUrlOriginal = e.Media.OriginalUrl
                })
            };
        }

        public static ReviewInviteDTO MapToDTO(this ReviewInvite entity)
        {
            return new ReviewInviteDTO()
            {
                Id = entity.Id,
                BookingId = entity.BookingId,
                ClientReviewed = entity.ClientReviewed,
                OpenAt = entity.OpenAt,
                CloseAt = entity.CloseAt,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
            };
        }

        public static void ApplyTo(this Review entity, ReviewUpdateRequest request)
        {
            if (entity.Status != ReviewStatus.Published && request.Status == ReviewStatus.Published)
            {
                entity.PublishedAt = DateTime.UtcNow;
            }

            entity.Status = request.Status;
            entity.Overall = request.Overall;
            entity.Accuracy = request.Accuracy;
            entity.CheckIn = request.CheckIn;
            entity.Communication = request.Communication;
            entity.Location = request.Location;
            entity.Value = request.Value;
            entity.Body = request.Body;
            entity.Locale = request.Locale;
        }
    }
}
