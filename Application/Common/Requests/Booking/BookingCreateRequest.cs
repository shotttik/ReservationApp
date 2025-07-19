using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class BookingCreateRequest
    {
        [Required]
        public int ClientID { get; set; }
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public required string ServiceName { get; set; }
        public string? Note { get; set; }
    }
}
