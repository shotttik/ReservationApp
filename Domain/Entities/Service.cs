namespace Domain.Entities
{
    public class Service
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Duration { get; set; } // in minutes
        public decimal Price { get; set; }
        public int CompanyID { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Company Company { get; set; } = null!;
        public void UpdateTimestamp() => UpdatedAt = DateTime.Now;
    }
}
