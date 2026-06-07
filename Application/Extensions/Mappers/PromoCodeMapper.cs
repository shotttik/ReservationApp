using Application.Common.Requests.Promo;
using Domain.DTO;
using Domain.Entities.Common;

namespace Application.Extensions.Mappers
{
    public static class PromoCodeMapper
    {
        public static PromoCodeDTO MapToDTO(this PromoCode entity)
        {
            return new PromoCodeDTO
            {
                Id = entity.Id,
                Code = entity.Code,
                DiscountAmount = entity.DiscountAmount,
                DiscountPercent = entity.DiscountPercent,
                ValidFrom = entity.ValidFrom,
                ValidTo = entity.ValidTo,
                MaxUsage = entity.MaxUsage,
                UsedCount = entity.UsedCount,
                CompanyId = entity.CompanyId,
                MinBookingPrice = entity.MinBookingPrice,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
        public static PromoCode MapToEntity(this PromoCodeCreateRequest request)
        {
            return new PromoCode
            {
                Code = request.Code,
                DiscountAmount = request.DiscountAmount,
                DiscountPercent = request.DiscountPercent,
                ValidFrom = request.ValidFrom,
                ValidTo = request.ValidTo,
                MaxUsage = request.MaxUsage,
                MinBookingPrice = request.MinBookingPrice
            };
        }

        public static void ApplyUpdate(this PromoCode entity, PromoCodeUpdateRequest request)
        {
            entity.Code = request.Code;
            entity.DiscountAmount = request.DiscountAmount;
            entity.DiscountPercent = request.DiscountPercent;
            entity.ValidFrom = request.ValidFrom;
            entity.ValidTo = request.ValidTo;
            entity.MaxUsage = request.MaxUsage;
            entity.ActiveStatus = request.ActiveStatus;
            entity.MinBookingPrice = request.MinBookingPrice;
        }
    }
}
