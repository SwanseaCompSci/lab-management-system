using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabScheduleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetLabScheduleSpecification : Specification<LabSchedule>, ISingleResultSpecification<LabSchedule>
    {
        public GetLabScheduleSpecification(Guid labScheduleId)
        {
            Query.Where(x => x.Id.Equals(labScheduleId));
        }
    }
}
