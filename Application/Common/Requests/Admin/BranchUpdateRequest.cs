using Domain.Enums;

namespace Application.Common.Requests.Admin
{
    public class BranchUpdateRequest() :BranchCreateRequest
    {
        public ActiveStatus ActiveStatus { get; set; }
    }
}
