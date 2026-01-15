using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Application.Common.Requests.Booking
{
    public class BookingGuestInfoContactUpdateRequest :IValidatableObject
    {
        public VerificationType ContactType { get; set; }
        [Required]
        [MaxLength(255)]
        public required string PendingNewContact { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ContactType == VerificationType.Email)
            {
                var emailValidator = new EmailAddressAttribute();
                if (!emailValidator.IsValid(PendingNewContact))
                {
                    yield return new ValidationResult(
                        "Contact must be a valid email address.",
                        new [] { nameof(PendingNewContact) }
                    );
                }
            }

            if (ContactType == VerificationType.Phone)
            {
                // optional: phone validation example
                if (!Regex.IsMatch(PendingNewContact, @"^\+?[1-9]\d{7,14}$"))
                {
                    yield return new ValidationResult(
                        "Contact must be a valid phone number.",
                        new [] { nameof(PendingNewContact) }
                    );
                }
            }
        }

    }
}
