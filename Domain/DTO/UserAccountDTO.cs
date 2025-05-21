using Domain.DTO.Company;
using Domain.DTO.WorkSchedule;
using Domain.Enums;

namespace Domain.DTO
{
    public class UserAccountDTO
    {
        public int ID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public Gender? Gender { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public required RoleDTO Role { get; set; }
        public UserCompanyDTO? Company { get; set; }
        public List<WorkScheduleDTO> WorkSchedules { get; set; } = [];
    }

    public class RoleDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public List<PermissionDTO> Permissions { get; set; } = [];
    }
    public class PermissionDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
    }
    public class UserCompanyDTO :CompanyDTO
    {
        public List<WorkScheduleDTO> WorkSchedules { get; set; } = [];
    }
}