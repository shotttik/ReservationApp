using Domain.Entities.Common;

namespace Domain.Entities.CompanyReleated
{
    public class CompanyFAQCategory :ActivableEntity
    {
        public string Name { get; set; } = null!;
        public int CompanyID { get; set; }
        public int Order { get; set; }
        public Company Company { get; set; } = null!;
        public ICollection<CompanyFAQ> FAQs { get; set; } = [];
    }
}
