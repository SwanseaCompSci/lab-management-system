using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Entities
{
    /// <summary>
    /// Represents a <see cref="Module"/> entity in a database.
    /// </summary>
    public class Module : AuditableEntity, IHasDomainEvent
    {
        /// <summary>
        /// Creates a new <see cref="Module"/> entity.
        /// </summary>
        /// <param name="name">Name of the module.</param>
        /// <param name="code">The module code.</param>
        /// <param name="level">The level of the module.</param>
        public Module(string name, string code, Level level)
        {
            Name = name;
            Code = code;
            Level = level;
        }

        /// <summary>
        /// A unique identifier of the entity.
        /// </summary>
        /// <remarks>
        /// The value is randomly generated during instantiation.
        /// </remarks>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// A name of the module.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A module code.
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// A level at which the module is taught.
        /// </summary>
        public Level Level { get; set; }

        /// <summary>
        /// A collection of <see cref="Lab"/>s in the module.
        /// </summary>
        public virtual ICollection<Lab> Labs { get; private set; } = new HashSet<Lab>();
        /// <summary>
        /// A collection of <see cref="UserModule"/>s in the modules.
        /// </summary>
        /// <remarks>
        /// <see cref="UserModule"/> provides information about associations between <see cref="User"/> and <see cref="Module"/>.
        /// </remarks>
        public virtual ICollection<UserModule> UserModules { get; private set; } = new HashSet<UserModule>();
        /// <summary>
        /// A collection of <see cref="ModulePreference"/>s associated with the module.
        /// </summary>
        public virtual ICollection<ModulePreference> ModulePreferences { get; set; } = new HashSet<ModulePreference>();

        #region IHasDomainEvent
        /// <inheritdoc/>
        public List<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();
        #endregion
    }
}
