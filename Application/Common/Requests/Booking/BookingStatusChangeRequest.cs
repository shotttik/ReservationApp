using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Common.Requests.Booking
{
    public class BookingStatusChangeRequest
    {
        public BookingStatus Status { get; set; }
        [JsonIgnore]
        public bool IsCompleted => Status == BookingStatus.Completed;
    }
}
