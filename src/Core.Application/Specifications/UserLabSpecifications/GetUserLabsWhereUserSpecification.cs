using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserLabsWhereUserSpecification : Specification<UserLab>
    {
        public GetUserLabsWhereUserSpecification(Guid userId)
        {
            Query.Where(x => x.UserId == userId);
            Query.Include(x => x.Lab);
        }
    }
}
