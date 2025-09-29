using Domain.Enums;

namespace Application.Common.Requests.Admin
{
    public class AssignUserToCompanyRequest
    {
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public Role Role { get; set; }
        public bool IsRoleValid => Role == Role.CompanyAdmin || Role == Role.CompanyEmployee;
    }
}
