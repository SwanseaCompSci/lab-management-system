using MediatR;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Common
{
    /// <summary>
    /// An interface for entities that support <see cref="DomainEvent"/>s.
    /// </summary>
    public interface IHasDomainEvent
    {
        /// <summary>
        /// A list of <see cref="DomainEvent"/>s to be published after saving the entity.
        /// </summary>
        public List<DomainEvent> DomainEvents { get; }
    }

    /// <summary>
    /// A base class for all Domain Events in the application.
    /// </summary>
    public abstract class DomainEvent : INotification
    {
        /// <summary>
        /// Creates a new instance of <see cref="DomainEvent"/>.
        /// </summary>
        protected DomainEvent() { }

        /// <summary>
        /// Indicates whether the event has been published or not.
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// UTC Date and Time when the event was created.
        /// </summary>
        public DateTime UtcOccurred { get; } = DateTime.UtcNow;
    }
}
