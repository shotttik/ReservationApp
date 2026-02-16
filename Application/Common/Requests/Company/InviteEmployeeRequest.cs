using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class InviteEmployeeRequest
    {
        [Required]
        public int UserAccountId { get; set; }
        [Required]
        public int BranchId { get; set; }
    }
}
