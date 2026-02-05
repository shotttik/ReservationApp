using Domain.DTO.Branch;
using Domain.DTO.WorkSchedule;
using Domain.Enums;

namespace Domain.DTO.Company
{
    public class CompanyDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string IN { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CompanyType Type { get; set; } = CompanyType.None;
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Viewed { get; set; }
        public required BranchDTO Branch { get; set; }
        public IEnumerable<WorkScheduleDTO> WorkSchedules { get; set; } = [];
        public IEnumerable<ServiceDTO> Services { get; set; } = [];
        public IEnumerable<MediaDTO> Media { get; set; } = [];
    }
}
