using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class AdminBookingCreateRequest :ClientBookingCreateRequest, IValidatableObject
    {
        public int? ClientId { get; set; }
        public BookingGuestInfoCreateRequest? GuestInfo { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Require at least one of ClientId or GuestInfo
            if (ClientId.HasValue && GuestInfo != null || GuestInfo == null && !ClientId.HasValue)
            {
                yield return new ValidationResult(
                    "Either ClientId or GuestInfo must be provided.");
            }
        }
    }
}
