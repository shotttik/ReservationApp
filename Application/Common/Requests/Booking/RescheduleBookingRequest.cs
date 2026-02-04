using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Booking
{
    public class RescheduleBookingRequest
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public int ServiceId { get; set; }
    }
}
