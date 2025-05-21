using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class InviteMemberRequest
    {
        [Required]
        public int UserAccountID { get; set; }
    }
}
