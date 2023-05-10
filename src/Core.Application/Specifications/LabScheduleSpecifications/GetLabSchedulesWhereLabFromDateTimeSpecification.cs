using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabScheduleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetLabSchedulesWhereLabFromDateTimeSpecification : Specification<LabSchedule>
    {
        public GetLabSchedulesWhereLabFromDateTimeSpecification(Guid labId, DateTime dateTime)
        {
            Query.Where(x => x.LabId == labId);
            Query.Where(x => x.Start >= dateTime);
        }
    }
}
