using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Admin
{
    public class AssignUserToCompanyRequest :IValidatableObject
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public Role Role { get; set; }
        public int? BranchId { get; set; }
        public bool IsRoleValid => Role == Role.CompanyAdmin || Role == Role.CompanyEmployee;
        public bool IsRoleCompanyEmployee => Role == Role.CompanyEmployee;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsRoleValid)
            {
                yield return new ValidationResult(
                    "Role must be either CompanyAdmin or CompanyEmployee.",
                    [nameof(Role)]
                );
            }

            if (Role == Role.CompanyEmployee)
            {
                if (BranchId == null)
                {
                    yield return new ValidationResult(
                        "BranchId is required for CompanyEmployee.",
                        [nameof(BranchId)]
                    );
                }
            }
            else
            {
                if (BranchId != null)
                {
                    yield return new ValidationResult(
                        "BranchId must be null for roles other than CompanyEmployee.",
                        [nameof(BranchId)]
                    );
                }
            }
        }
    }
}
