using Domain.Enums;

namespace Domain.DTO.Company
{
    public class CompanyDTOGeneral
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string IN { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CompanyType Type { get; set; } = CompanyType.None;
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Viewed { get; set; }
    }
}
