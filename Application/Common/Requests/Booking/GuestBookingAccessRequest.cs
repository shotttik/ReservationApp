using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class GuestBookingAccessRequest
    {
        [Required]
        public required string Reference { get; set; }
        [Required]
        public required string Contact { get; set; }
        public void Deconstruct(out string reference, out string contact)
        {
            reference = Reference;
            contact = Contact;
        }

    }
}
