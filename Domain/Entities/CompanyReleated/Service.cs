using Domain.Entities.Common;

namespace Domain.Entities.CompanyReleated
{
    public class Service :ActivableEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Duration { get; set; } // in minutes
        public decimal Price { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; } = null!;
    }
}
