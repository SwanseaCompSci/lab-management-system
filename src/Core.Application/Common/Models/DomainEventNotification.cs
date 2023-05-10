using MediatR;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models
{
    /// <summary>
    /// Represents a notification for a single <see cref="DomainEvent"/>.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of <see cref="DomainEvent"/>.</typeparam>
    public sealed class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    {
        /// <summary>
        /// Creates a new instance of <see cref="DomainEventNotification{TDomainEvent}"/>.
        /// </summary>
        /// <param name="domainEvent">The <see cref="DomainEvent"/> to be published from the notification.</param>
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            Event = domainEvent;
        }

        /// <summary>
        /// The event in the notification.
        /// </summary>
        public TDomainEvent Event { get; }
    }
}
