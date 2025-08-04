using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class ClientBookingCreateRequest
    {
        [Required]
        public int EmployeeID { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public int ServiceID { get; set; }
        public string? Note { get; set; }
    }
}
