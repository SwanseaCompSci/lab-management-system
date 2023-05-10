using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="TimeAvailability"/> entity in a database.
    /// </summary>
    public class TimeAvailability : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="TimeAvailability"/> entity.
        /// </summary>
        /// <param name="userId">An identifier of the associated user.</param>
        /// <param name="day">A day of week of the time availability.</param>
        /// <param name="startTime">A start time of the time availability.</param>
        /// <param name="endTime">An end time of the time availability.</param>
        public TimeAvailability(Guid userId, WorkDayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            UserId = userId;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// A unique identifier of the entity.
        /// </summary>
        /// <remarks>
        /// The value is randomly generated during instantiation.
        /// </remarks>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// An identifier of the associated <see cref="Entities.User"/>.
        /// </summary>
        public Guid UserId { get; }
        /// <summary>
        /// The associated <see cref="Entities.User"/>.
        /// </summary>
        public virtual User User { get; private set; } = null!;

        /// <summary>
        /// A day of week.
        /// </summary>
        public WorkDayOfWeek Day { get; }
        /// <summary>
        /// Start time.
        /// </summary>
        public TimeOnly StartTime { get; }
        /// <summary>
        /// End time.
        /// </summary>
        public TimeOnly EndTime { get; }

        /// <summary>
        /// Marks if the <see cref="TimeAvailability"/> is allocated or not.
        /// </summary>
        public bool IsAllocated { get; set; }

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
