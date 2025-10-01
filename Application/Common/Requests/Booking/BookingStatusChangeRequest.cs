using Domain.Enums;
using System.Text.Json.Serialization;

namespace Application.Common.Requests.Booking
{
    public class BookingStatusChangeRequest
    {
        public BookingStatus Status { get; set; }
        public string? CancellationReason { get; set; }
        [JsonIgnore]
        public bool IsCompleted => Status == BookingStatus.Completed;
        [JsonIgnore]
        public bool IsCanceled => Status == BookingStatus.Canceled;
        [JsonIgnore]
        public bool IsFailed => Status == BookingStatus.Failed;
    }
}
