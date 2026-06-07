using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Requests.Promo
{
    public class PromoCodeCreateRequest :IValidatableObject
    {
        private string _code = null!;
        [Required]
        [MaxLength(30)]
        [NotNull]
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
        public int? MinBookingPrice { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Exactly one must be set
            if ((DiscountAmount.HasValue && DiscountPercent.HasValue) ||
                (!DiscountAmount.HasValue && !DiscountPercent.HasValue))
            {
                yield return new ValidationResult(
                    "Either DiscountAmount OR DiscountPercent must be provided (but not both).",
                    new [] { nameof(DiscountAmount), nameof(DiscountPercent) });
            }

            // ValidTo must be after ValidFrom
            if (ValidTo <= ValidFrom)
            {
                yield return new ValidationResult(
                    "ValidTo must be greater than ValidFrom.",
                    new [] { nameof(ValidTo) });
            }
            // Percent range check
            if (DiscountPercent.HasValue &&
                (DiscountPercent <= 0 || DiscountPercent > 100))
            {
                yield return new ValidationResult(
                    "DiscountPercent must be between 0 and 100.",
                    new [] { nameof(DiscountPercent) });
            }

            // Amount must be positive
            if (DiscountAmount.HasValue && DiscountAmount <= 0)
            {
                yield return new ValidationResult(
                    "DiscountAmount must be greater than 0.",
                    new [] { nameof(DiscountAmount) });
            }

            // Min booking must be positive
            if (MinBookingPrice.HasValue && MinBookingPrice <= 0)
            {
                yield return new ValidationResult(
                    "MinBookingPrice must be greater than 0.",
                    new [] { nameof(MinBookingPrice) });
            }
        }
    }
}
