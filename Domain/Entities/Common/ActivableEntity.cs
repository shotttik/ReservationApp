using Domain.Enums;
using Domain.Interfaces.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Common
{
    public abstract class ActivableEntity :BaseEntity, IActivableEntity
    {
        public ActiveStatus ActiveStatus { get; set; } = ActiveStatus.Active;
        public DateTime? StatusChangedAt { get; set; }

        public void Activate()
        {
            ActiveStatus = ActiveStatus.Active;
            StatusChangedAt = DateTime.UtcNow;
        }

        public void Disable()
        {
            ActiveStatus = ActiveStatus.Disabled;
            StatusChangedAt = DateTime.UtcNow;
        }
        public bool IsActive => ActiveStatus == ActiveStatus.Active;
        public bool IsDisabled => ActiveStatus == ActiveStatus.Disabled;
    }
}
