using Domain.Entities.Common;

namespace Domain.Entities.User
{
    public class UserAccountMedia
    {
        public int UserAccountId { get; set; }
        public int MediaId { get; set; }
        public virtual UserAccount UserAccount { get; set; } = default!;
        public virtual Media Media { get; set; } = default!;
    }
}
