using Domain.Entities.Common;

namespace Domain.Entities.CompanyReleated
{
    public class CompanyMedia
    {
        public int CompanyID { get; set; }
        public int MediaID { get; set; }
        public virtual Company Company { get; set; } = default!;
        public virtual Media Media { get; set; } = default!;
        public bool IsMain { get; set; }
    }
}
