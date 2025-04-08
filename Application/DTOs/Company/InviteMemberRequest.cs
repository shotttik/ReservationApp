using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company
{
    public class InviteMemberRequest
    {
        [Required]
        public int UserAccountID { get; set; }
    }
}
