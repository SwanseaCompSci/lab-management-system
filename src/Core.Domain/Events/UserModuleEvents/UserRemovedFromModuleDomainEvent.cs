using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserModuleEvents
{
    // TODO: Add docs comments
    public sealed class UserRemovedFromModuleDomainEvent : DomainEvent
    {
        public UserRemovedFromModuleDomainEvent(Guid userId, Guid moduleId)
        {
            UserId = userId;
            ModuleId = moduleId;
        }

        public Guid UserId { get; }
        public Guid ModuleId { get; }
    }
}
