using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserEvents
{
    // TODO: Add docs comments
    public sealed class UserTimeAvailabilitySubmittedDomainEvent : DomainEvent
    {
        public UserTimeAvailabilitySubmittedDomainEvent(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; }
    }
}
