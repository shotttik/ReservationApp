using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.Requests.Company
{
    public class EmployeeUpdateRequest
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(200)]
        public string? LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public int? BranchId { get; set; }
    }
}
