using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabEvents
{
    // TODO: Add docs comments
    public sealed class LabUpdatedDomainEvent : DomainEvent
    {
        public LabUpdatedDomainEvent(Lab oldLab, Lab newLab)
        {
            OldLab = oldLab;
            NewLab = newLab;
        }

        public Lab OldLab { get; }
        public Lab NewLab { get; }
    }
}
