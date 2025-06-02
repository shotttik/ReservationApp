using Domain.DTO.Company;
using Domain.DTO.WorkSchedule;
using Domain.Enums;
using System.Text.Json.Serialization;

namespace Domain.DTO.User
{
    public class UserAccountDTO
    {
        public virtual int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public required RoleDTO Role { get; set; }
        public UserCompanyDTO? Company { get; set; }
        public List<WorkScheduleDTO> WorkSchedules { get; set; } = [];
        [JsonIgnore]
        public bool IsPublicUser => Role.ID == Entities.Role.PublicUser.ID;
    }
    public class UserCompanyDTO :CompanyDTO
    {
        public List<WorkScheduleDTO> WorkSchedules { get; set; } = [];
    }
}