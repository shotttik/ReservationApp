using Domain.DTO.Company;
using Domain.DTO.WorkSchedule;
using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.DTO.User
{
    public class UserAccountDTO
    {
        public virtual int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public required RoleDTO Role { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public string? ProfileImageUrlWebp { get; set; }
        public string? ProfileImageUrlOriginal { get; set; }
        public List<WorkScheduleDTO> WorkSchedules { get; set; } = [];
        public List<ServiceDTO> Services { get; set; } = [];
        [JsonIgnore]
        public bool IsPublicUser => Role.Id == (int)Enums.Role.PublicUser;
        [JsonIgnore]
        public bool IsSuperUser => Role.Id == (int)Enums.Role.SuperAdmin;
        [JsonIgnore]
        public bool IsCompanyAdmin => Role.Id == (int)Enums.Role.CompanyAdmin;
        [JsonIgnore]
        public bool IsCompanyEmployee => Role.Id == (int)Enums.Role.CompanyEmployee;
    }
}