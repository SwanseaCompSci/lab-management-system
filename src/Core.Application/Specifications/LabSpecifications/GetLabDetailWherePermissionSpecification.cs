using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabSpecifications
{
    // TODO: Add docs comments
    public sealed class GetLabDetailWherePermissionSpecification : Specification<Lab>, ISingleResultSpecification<Lab>
    {
        public GetLabDetailWherePermissionSpecification(Guid labId, Guid userId)
        {
            Query.Where(x => x.Id.Equals(labId));
            Query.Include(x => x.Module).ThenInclude(x => x.UserModules);
            Query.Where(x => x.Module.UserModules.Select(x => x.UserId).Contains(userId));
            Query.Include(x => x.UserLabs).ThenInclude(x => x.User);
            Query.Include(x => x.LabSchedules);
            Query.Take(1);
        }
    }
}
