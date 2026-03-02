namespace Domain.DTO.Company
{
    public struct CompanyMediaDTO
    {
        public int Id { get; set; }
        public bool IsMain { get; set; }
        public string ImageUrlWebp { get; set; }
        public string ImageUrlOriginal { get; set; }
    }
}
