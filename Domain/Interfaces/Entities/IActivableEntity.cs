using Domain.Enums;

namespace Domain.Interfaces.Entities
{
    public interface IActivableEntity
    {
        ActiveStatus ActiveStatus { get; set; }
        DateTime? StatusChangedAt { get; set; }
        void Activate();
        void Disable();
        bool IsActive { get; }
        bool IsDisabled { get; }
    }
}
