using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Admin
{
    public class UserUpdateRequest :IValidatableObject
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(200)]
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public Role? Role { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public ActiveStatus? ActiveStatus { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Role)
            {
                case null:
                    break;
                case Domain.Enums.Role.PublicUser:
                    if (CompanyId != null || BranchId != null)
                    {
                        yield return new ValidationResult(
                            "PublicUser must not have CompanyId or BranchId.",
                            [nameof(CompanyId), nameof(BranchId)]
                        );
                    }
                    break;

                case Domain.Enums.Role.CompanyAdmin:
                    if (CompanyId == null)
                    {
                        yield return new ValidationResult(
                            "CompanyId is required for CompanyAdmin.",
                            [nameof(CompanyId)]
                        );
                    }

                    if (BranchId != null)
                    {
                        yield return new ValidationResult(
                            "BranchId must not be provided for CompanyAdmin.",
                            [nameof(BranchId)]
                        );
                    }
                    break;

                case Domain.Enums.Role.CompanyEmployee:
                    if (CompanyId == null)
                    {
                        yield return new ValidationResult(
                            "CompanyId is required for CompanyEmployee.",
                            [nameof(CompanyId)]
                        );
                    }

                    if (BranchId == null)
                    {
                        yield return new ValidationResult(
                            "BranchId is required for CompanyEmployee.",
                            [nameof(BranchId)]
                        );
                    }
                    break;

                case Domain.Enums.Role.SuperAdmin:
                    if (CompanyId != null || BranchId != null)
                    {
                        yield return new ValidationResult(
                            "SuperAdmin must not have CompanyId or BranchId.",
                            [nameof(CompanyId), nameof(BranchId)]
                        );
                    }
                    break;

                default:
                    yield return new ValidationResult(
                        "Invalid role.",
                        [nameof(Role)]
                    );
                    break;
            }
        }
    }
}
