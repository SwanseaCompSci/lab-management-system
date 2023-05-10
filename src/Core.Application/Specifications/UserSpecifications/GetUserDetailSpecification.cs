using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserDetailSpecification : Specification<User>, ISingleResultSpecification<User>
    {
        public GetUserDetailSpecification(Guid userId)
        {
            Query.Where(x => x.Id.Equals(userId));

            Query.Include(x => x.ModulePreferences).ThenInclude(x => x.Module);
            Query.Include(x => x.UserModules).ThenInclude(x => x.Module);
            Query.Include(x => x.TimeAvailabilities);
        }
    }
}
