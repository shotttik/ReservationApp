using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Company
{
    public class InviteMemberRequest
    {
        [Required]
        public int MemberID { get; set; }
    }
}
