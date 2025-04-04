using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class InviteMemberRequest
    {
        [Required]
        public int MemberID { get; set; }
    }
}
