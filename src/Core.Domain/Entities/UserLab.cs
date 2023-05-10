using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="UserLab"/> entity in a database.
    /// </summary>
    public class UserLab : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="UserLab"/> entity.
        /// </summary>
        /// <param name="userId">An identifier of the associated <see cref="Entities.User"/>.</param>
        /// <param name="labId">An identifier of the associated <see cref="Entities.Lab"/>.</param>
        public UserLab(Guid userId, Guid labId)
        {
            UserId = userId;
            LabId = labId;
        }

        /// <summary>
        /// An identifier of the associated <see cref="Entities.User"/>.
        /// </summary>
        public Guid UserId { get; }
        /// <summary>
        /// A <see cref="Entities.User"/> associated with the <see cref="Entities.Lab"/>.
        /// </summary>
        public virtual User User { get; private set; } = null!;

        /// <summary>
        /// An identifier of the associated <see cref="Entities.Lab"/>.
        /// </summary>
        public Guid LabId { get; }
        /// <summary>
        /// A <see cref="Entities.Lab"/> associated with the <see cref="Entities.User"/>.
        /// </summary>
        public virtual Lab Lab { get; private set; } = null!;

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
