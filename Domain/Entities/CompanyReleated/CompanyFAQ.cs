using Domain.Entities.Common;

namespace Domain.Entities.CompanyReleated
{
    public class CompanyFAQ :ActivableEntity
    {
        public required string Question { get; set; }
        public required string Answer { get; set; }
        public int Order { get; set; }
        public int CategoryID { get; set; }
        public CompanyFAQCategory Category { get; set; } = null!;
    }
}
