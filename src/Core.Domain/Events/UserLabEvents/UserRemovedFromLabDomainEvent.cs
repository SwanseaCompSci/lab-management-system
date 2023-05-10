using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserLabEvents
{
    // TODO: Add docs comments
    public sealed class UserRemovedFromLabDomainEvent : DomainEvent
    {
        public UserRemovedFromLabDomainEvent(Guid userId, Guid labId)
        {
            UserId = userId;
            LabId = labId;
        }

        public Guid UserId { get; }
        public Guid LabId { get; }
    }
}
