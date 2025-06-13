namespace Domain.DTO.Company
{
    public struct CompanyFAQCategoryDTO
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public int Order { get; set; }
        public int CompanyID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
