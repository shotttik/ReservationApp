namespace Domain.Interfaces.Entities
{
    public interface IBaseEntity
    {
        int ID { get; set; }
        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        void UpdateTimestamp();
    }
}
