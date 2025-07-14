using Domain.Enums;

namespace Domain.DTO.Company
{
    public struct CompanyFAQDTO
    {
        public int ID { get; set; }
        public required string Question { get; set; }
        public string Answer { get; set; }
        public int Order { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int CategoryID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
