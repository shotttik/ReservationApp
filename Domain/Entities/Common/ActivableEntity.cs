using Domain.Enums;
using Domain.Interfaces.Entities;

namespace Domain.Entities.Common
{
    public abstract class ActivableEntity :BaseEntity, IActivableEntity
    {
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public void Activate()
        {
            ActiveStatus = ActiveStatus.Active;
            UpdateTimestamp();
        }

        public void Disable()
        {
            ActiveStatus = ActiveStatus.Disabled;
            UpdateTimestamp();
        }
        public bool IsActive => ActiveStatus == ActiveStatus.Active;
        public bool IsDisabled => ActiveStatus == ActiveStatus.Disabled;
    }
}
