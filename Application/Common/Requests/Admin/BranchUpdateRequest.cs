using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Admin
{
    public class BranchUpdateRequest() :BranchCreateRequest
    {
        [Required]
        public ActiveStatus ActiveStatus { get; set; }
    }
}
