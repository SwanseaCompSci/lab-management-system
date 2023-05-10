using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserSpecification : Specification<User>, ISingleResultSpecification<User>
    {
        public GetUserSpecification(Guid userId)
        {
            Query.Where(x => x.Id.Equals(userId));
            Query.Take(1);
        }
    }
}
