using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services
{
    /// <summary>
    /// Defines a contract for a service that publishes instances of <see cref="DomainEvent"/>.
    /// </summary>
    public interface IDomainEventService
    {
        /// <summary>
        /// Publishes a <see cref="DomainEvent"/>.
        /// </summary>
        /// <param name="domainEvent">The <see cref="DomainEvent"/> to publish.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task Publish(DomainEvent domainEvent);
    }
}
