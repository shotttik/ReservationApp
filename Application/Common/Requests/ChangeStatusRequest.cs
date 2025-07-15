using Domain.Enums;

namespace Application.Common.Requests
{
    public class ChangeStatusRequest
    {
        public ActiveStatus NewStatus { get; set; }
    }
}
