using Domain.Enums;

namespace Domain.DTO.Company
{
    public struct CompanyFAQCategoryDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Order { get; set; }
        public int CompanyId { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
