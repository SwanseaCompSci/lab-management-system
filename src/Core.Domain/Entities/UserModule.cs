using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="UserModule"/> entity in a database.
    /// </summary>
    public class UserModule : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="UserModule"/> entity.
        /// </summary>
        /// <param name="userId">An identifier of the associated <see cref="Entities.User"/>.</param>
        /// <param name="moduleId">An identifier of the associated <see cref="Entities.Module">.</param>
        /// <param name="role">A <see cref="ModuleRole"/> associated to the <see cref="Entities.User"/> for <see cref="Entities.Module"/>.</param>
        public UserModule(Guid userId, Guid moduleId, ModuleRole role)
        {
            UserId = userId;
            ModuleId = moduleId;
            Role = role;
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
        /// A <see cref="ModuleRole"/> associated to the <see cref="User"/> for the <see cref="Module"/>.
        /// </summary>
        public ModuleRole Role { get; set; }

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
