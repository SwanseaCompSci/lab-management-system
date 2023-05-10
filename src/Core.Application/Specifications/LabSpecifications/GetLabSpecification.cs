using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabSpecifications
{
    // TODO: Add docs comments
    public sealed class GetLabSpecification : Specification<Lab>, ISingleResultSpecification<Lab>
    {
        public GetLabSpecification(Guid labId)
        {
            Query.Where(x => x.Id.Equals(labId));
            Query.Take(1);
        }
    }
}
