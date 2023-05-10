using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabSpecifications
{
    // TODO: Add docs comments
    public sealed class GetLabDetailSpecification : Specification<Lab>, ISingleResultSpecification<Lab>
    {
        public GetLabDetailSpecification(Guid labId)
        {
            Query.Where(x => x.Id.Equals(labId));
            Query.Include(x => x.UserLabs).ThenInclude(x => x.User);
            Query.Include(x => x.LabSchedules);
            Query.Take(1);
        }
    }
}
