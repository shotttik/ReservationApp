using Domain.DTO.Branch;
using Domain.DTO.WorkSchedule;

namespace Domain.DTO.Company
{
    public class CompanyDTO :CompanyDTOGeneral
    {
        public string? Description { get; set; }
        public CompanySubscriptionDTO? Subscription { get; set; }
        public IEnumerable<BranchDTO> Branches { get; set; } = [];
        public IEnumerable<WorkScheduleDTO> WorkSchedules { get; set; } = [];
        public IEnumerable<ServiceDTO> Services { get; set; } = [];
        public IEnumerable<CompanyMediaDTO> Media { get; set; } = [];
    }
}
