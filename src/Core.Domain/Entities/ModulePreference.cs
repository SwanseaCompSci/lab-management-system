using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="ModulePreference"/> entity in a database.
    /// </summary>
    public class ModulePreference : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="ModulePreference"/> entity.
        /// </summary>
        /// <param name="userId">An identifier of the associated <see cref="Entities.User"/>.</param>
        /// <param name="moduleId">An identifier of the associated <see cref="Entities.Module">.</param>
        public ModulePreference(Guid userId, Guid moduleId)
        {
            UserId = userId;
            ModuleId = moduleId;
        }

        /// <summary>
        /// An identifier of the associated <see cref="Entities.User"/>.
        /// </summary>
        public Guid UserId { get; }
        /// <summary>
        /// A <see cref="Entities.User"/> associated with the <see cref="Entities.Module"/>.
        /// </summary>
        public virtual User User { get; private set; } = null!;

        /// <summary>
        /// An identifier of the associated <see cref="Entities.Module"/>.
        /// </summary>
        public Guid ModuleId { get; }
        /// <summary>
        /// A <see cref="Entities.Module"/> associated with the <see cref="Entities.User"/>.
        /// </summary>
        public virtual Module Module { get; private set; } = null!;

        /// <summary>
        /// Module preference request status.
        /// </summary>
        public Status Status { get; set; } = Status.PendingResponse;

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
