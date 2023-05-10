using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabScheduleEvents
{
    // TODO: Add docs comments
    public sealed class LabScheduleCreatedDomainEvent : DomainEvent
    {
        public LabScheduleCreatedDomainEvent(LabSchedule labSchedule)
        {
            LabSchedule = labSchedule;
        }

        public LabSchedule LabSchedule { get; }
    }
}
