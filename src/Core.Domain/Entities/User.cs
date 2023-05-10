using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="User"/> entity in a database.
    /// </summary>
    public class User : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="User"/> entity.
        /// </summary>
        /// <param name="id">An identifier of the user.</param>
        /// <param name="firstName">A first name of the user.</param>
        /// <param name="surname">A surname of the user.</param>
        /// <param name="achievedLevel">The highest level of education the user achieved.</param>
        public User(Guid id, string firstName, string surname, Level achievedLevel, int maxWeeklyWorkHours)
        {
            Id = id;
            FirstName = firstName;
            Surname = surname;
            AchievedLevel = achievedLevel;
            MaxWeeklyWorkHours = maxWeeklyWorkHours;
        }

        /// <summary>
        /// A unique identifier of the entity.
        /// </summary>
        public Guid Id { get; }
        /// <summary>
        /// A first name of the user.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// A surname of the user.
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// The highest level of education the user achieved.
        /// </summary>
        public Level AchievedLevel { get; set; }
        /// <summary>
        /// The maximum number of hours per week the user is allowed to work.
        /// </summary>
        public int MaxWeeklyWorkHours { get; set; }
        /// <summary>
        /// A token for a questionnaire.
        /// </summary>
        public Guid? QuestionnaireToken { get; set; }

        /// <summary>
        /// A collection of user's <see cref="ModulePreference"/>s.
        /// </summary>
        public virtual ICollection<ModulePreference> ModulePreferences { get; set; } = new HashSet<ModulePreference>();
        /// <summary>
        /// A collection of <see cref="UserModule"/>s associated to the user.
        /// </summary>
        public virtual ICollection<UserModule> UserModules { get; private set; } = new HashSet<UserModule>();
        /// <summary>
        /// A collection of <see cref="UserLab"/>s associated to the user.
        /// </summary>
        public virtual ICollection<UserLab> UserLabs { get; private set; } = new HashSet<UserLab>();
        /// <summary>
        /// A collection of <see cref="UserLabSchedule"/>s associated to the user.
        /// </summary>
        public virtual ICollection<UserLabSchedule> UserLabSchedules { get; private set; } = new HashSet<UserLabSchedule>();
        /// <summary>
        /// A collection of <see cref="TimeAvailability"/> entries of the user.
        /// </summary>
        public virtual ICollection<TimeAvailability> TimeAvailabilities { get; private set; } = new HashSet<TimeAvailability>();

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
