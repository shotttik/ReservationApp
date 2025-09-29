using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class InviteEmployeeRequest
    {
        [Required]
        public int UserAccountID { get; set; }
    }
}
