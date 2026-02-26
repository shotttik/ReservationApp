using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.SubscriptionPlan
{
    public class ActivateSubscriptionRequest :IValidatableObject
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate >= EndDate)
            {
                yield return new ValidationResult(
                    "Start date must be earlier than end date.",
                    new [] { nameof(StartDate), nameof(EndDate) });
            }
        }
    }
}
