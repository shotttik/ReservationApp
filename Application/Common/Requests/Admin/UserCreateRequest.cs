using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Admin
{
    public class UserCreateRequest :IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(200)]
        public required string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required Role Role { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (Role)
            {
                case Role.PublicUser:
                    if (CompanyId != null || BranchId != null)
                    {
                        yield return new ValidationResult(
                            "PublicUser must not have CompanyId or BranchId.",
                            [nameof(CompanyId), nameof(BranchId)]
                        );
                    }
                    break;

                case Role.CompanyAdmin:
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

                case Role.CompanyEmployee:
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

                case Role.SuperAdmin:
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
