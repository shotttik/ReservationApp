using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Promo
{
    public class PromoCodeUpdateRequest :IValidatableObject
    {
        private string _code = null!;
        [Required]
        [MaxLength(30)]
        public string Code
        {
            get => _code;
            set => _code = value?.Trim() ?? string.Empty;
        }
        [Range(0.01, double.MaxValue)]
        public decimal? DiscountAmount { get; set; }
        [Range(0.01, 100)]
        public decimal? DiscountPercent { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int? MaxUsage { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int? MinBookingAmount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Exactly one discount type must be provided
            if ((DiscountAmount.HasValue && DiscountPercent.HasValue) ||
                (!DiscountAmount.HasValue && !DiscountPercent.HasValue))
            {
                yield return new ValidationResult(
                    "Either DiscountAmount OR DiscountPercent must be provided (but not both).",
                    new [] { nameof(DiscountAmount), nameof(DiscountPercent) });
            }

            // Date validation
            if (ValidTo <= ValidFrom)
            {
                yield return new ValidationResult(
                    "ValidTo must be greater than ValidFrom.",
                    new [] { nameof(ValidTo) });
            }

            // Discount percent validation
            if (DiscountPercent.HasValue &&
                (DiscountPercent <= 0 || DiscountPercent > 100))
            {
                yield return new ValidationResult(
                    "DiscountPercent must be between 0 and 100.",
                    new [] { nameof(DiscountPercent) });
            }

            // Discount amount validation
            if (DiscountAmount.HasValue && DiscountAmount <= 0)
            {
                yield return new ValidationResult(
                    "DiscountAmount must be greater than 0.",
                    new [] { nameof(DiscountAmount) });
            }

            // MaxUsage validation
            if (MaxUsage.HasValue && MaxUsage <= 0)
            {
                yield return new ValidationResult(
                    "MaxUsage must be greater than 0.",
                    new [] { nameof(MaxUsage) });
            }

            // MinBookingAmount validation
            if (MinBookingAmount.HasValue && MinBookingAmount <= 0)
            {
                yield return new ValidationResult(
                    "MinBookingAmount must be greater than 0.",
                    new [] { nameof(MinBookingAmount) });
            }
        }
    }
}
