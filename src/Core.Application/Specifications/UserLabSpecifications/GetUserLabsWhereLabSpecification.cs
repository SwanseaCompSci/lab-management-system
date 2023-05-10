using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserLabsWhereLabSpecification : Specification<UserLab>
    {
        public GetUserLabsWhereLabSpecification(Guid labId)
        {
            Query.Where(x => x.LabId.Equals(labId));
        }
    }
}
