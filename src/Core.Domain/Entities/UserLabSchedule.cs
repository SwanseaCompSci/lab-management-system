using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="UserLabSchedule"/> entity in a database.
    /// </summary>
    public class UserLabSchedule : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="UserLabSchedule"/> entity.
        /// </summary>
        /// <param name="userId">An identifier of the associated <see cref="Entities.User"/>.</param>
        /// <param name="labScheduleId">An identifier of the associated <see cref="Entities.LabSchedule"/>.</param>
        public UserLabSchedule(Guid userId, Guid labScheduleId)
        {
            UserId = userId;
            LabScheduleId = labScheduleId;
        }

        /// <summary>
        /// An identifier of the associated <see cref="Entities.User"/>.
        /// </summary>
        public Guid UserId { get; }
        /// <summary>
        /// A <see cref="Entities.User"/> associated with the <see cref="Entities.LabSchedule"/>.
        /// </summary>
        public virtual User User { get; private set; } = null!;

        /// <summary>
        /// An identifier of the associated <see cref="Entities.LabSchedule"/>.
        /// </summary>
        public Guid LabScheduleId { get; }
        /// <summary>
        /// A <see cref="Entities.LabSchedule"/> associated with the <see cref="Entities.User"/>.
        /// </summary>
        public virtual LabSchedule LabSchedule { get; private set; } = null!;

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
