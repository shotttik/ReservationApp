using Domain.Interfaces.Entities;

namespace Domain.Entities.Common
{
    public class BaseEntity :IBaseEntity
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;
    }
}
