using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class UpdateBookingNoteRequest
    {
        [Required]
        public string Note { get; set; } = string.Empty;
    }
}
