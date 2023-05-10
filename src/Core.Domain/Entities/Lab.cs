using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="Lab"/> entity in a database.
    /// </summary>
    public class Lab : AuditableEntity, IHasDomainEvent, ICloneable
    {
        /// <summary>
        /// Creates a new <see cref="Lab"/> entity.
        /// </summary>
        /// <param name="moduleId">An identifier of the associated <see cref="Module"/>.</param>
        /// <param name="name">A name of the lab.</param>
        /// <param name="day">A day of week when the lab takes place.</param>
        /// <param name="startTime">A start time of the lab.</param>
        /// <param name="endTime">An end time of the lab.</param>
        /// <param name="minNumberOfStaff">The minimum number of staff members to run the lab.</param>
        /// <param name="maxNumberOfStaff">The maximum number of staff members to run the lab.</param>
        public Lab(Guid moduleId,
                   string name,
                   WorkDayOfWeek day,
                   TimeOnly startTime,
                   TimeOnly endTime,
                   int minNumberOfStaff,
                   int maxNumberOfStaff)
        {
            ModuleId = moduleId;
            Name = name;
            Day = day;
            StartTime = startTime;
            EndTime = endTime;
            MinNumberOfStaff = minNumberOfStaff;
            MaxNumberOfStaff = maxNumberOfStaff;
        }

        /// <summary>
        /// A unique identifier of the entity.
        /// </summary>
        /// <remarks>
        /// The value is randomly generated during instantiation.
        /// </remarks>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// An identifier of the associated <see cref="Entities.Module"/>.
        /// </summary>
        public Guid ModuleId { get; }
        /// <summary>
        /// A <see cref="Entities.Module"/> associated with the lab.
        /// </summary>
        public virtual Module Module { get; private set; } = null!;

        /// <summary>
        /// A name of the lab.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A day of week when the lab takes place.
        /// </summary>
        public WorkDayOfWeek Day { get; set; }

        /// <summary>
        /// A start time of the lab.
        /// </summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>
        /// An end time of the lab.
        /// </summary>
        public TimeOnly EndTime { get; set; }

        /// <summary>
        /// The minimum number of staff members to run the lab.
        /// </summary>
        public int MinNumberOfStaff { get; set; }

        /// <summary>
        /// The maximum number of staff members to run the lab.
        /// </summary>
        public int MaxNumberOfStaff { get; set; }

        /// <summary>
        /// A collection of <see cref="LabSchedule"/>s of the lab.
        /// </summary>
        public virtual ICollection<LabSchedule> LabSchedules { get; private set; } = new HashSet<LabSchedule>();

        /// <summary>
        /// A collection of <see cref="UserLab"/>s of the lab.
        /// </summary>
        /// <remarks>
        /// <see cref="UserLab"/> provides information about associations between <see cref="User"/> and <see cref="Lab"/>.
        /// </remarks>
        public virtual ICollection<UserLab> UserLabs { get; private set; } = new HashSet<UserLab>();

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion

        #region ICloneable
        /// <inheritdoc/>
        public object Clone()
        {
            return new Lab(moduleId: ModuleId,
                           name: Name,
                           day: Day,
                           startTime: StartTime,
                           endTime: EndTime,
                           minNumberOfStaff: MinNumberOfStaff,
                           maxNumberOfStaff: MaxNumberOfStaff)
            {
                Id = Id,
                LabSchedules = LabSchedules,
                UserLabs = UserLabs,
                DomainEvents = DomainEvents,
            };
        }
        #endregion
    }
}
