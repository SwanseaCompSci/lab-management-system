using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="LabSchedule"/> entity in a database.
    /// </summary>
    public class LabSchedule : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="LabSchedule"/> entity.
        /// </summary>
        /// <param name="labId">An identifier of the associated <see cref="Entities.Lab"/>.</param>
        /// <param name="start">A start <see cref="DateTime"/> of the lab schedule.</param>
        /// <param name="end">An end <see cref="DateTime"/> of the lab schedule.</param>
        public LabSchedule(Guid labId, DateTime start, DateTime end)
        {
            LabId = labId;
            Start = start;
            End = end;
        }

        /// <summary>
        /// A unique identifier of the entity.
        /// </summary>
        /// <remarks>
        /// The value is randomly generated during instantiation.
        /// </remarks>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// An identifier of the associated <see cref="Entities.Lab"/>.
        /// </summary>
        public Guid LabId { get; }
        /// <summary>
        /// A <see cref="Entities.Lab"/> associated with the lab schedule.
        /// </summary>
        public virtual Lab Lab { get; private set; } = null!;

        /// <summary>
        /// A start time of the lab schedule.
        /// </summary>
        public DateTime Start { get; set; }
        /// <summary>
        /// An end time of the lab schedule.
        /// </summary>
        public DateTime End { get; set; }

        /// <summary>
        /// A collection of <see cref="UserLabSchedule"/>s of the lab.
        /// </summary>
        /// <remarks>
        /// <see cref="UserLabSchedule"/> provides information about associations between <see cref="User"/> and <see cref="LabSchedule"/>.
        /// </remarks>
        public virtual ICollection<UserLabSchedule> UserLabSchedules { get; private set; } = new HashSet<UserLabSchedule>();

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
