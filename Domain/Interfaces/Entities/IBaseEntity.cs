namespace Domain.Interfaces.Entities
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        void UpdateTimestamp();
    }
}
