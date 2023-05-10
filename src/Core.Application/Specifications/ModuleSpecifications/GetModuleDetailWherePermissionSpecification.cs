using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetModuleDetailWherePermissionSpecification : Specification<Module>, ISingleResultSpecification<Module>
    {
        public GetModuleDetailWherePermissionSpecification(Guid moduleId, Guid userId)
        {
            Query.Where(x => x.Id.Equals(moduleId));
            Query.Include(x => x.ModulePreferences).ThenInclude(x => x.User);
            Query.Include(x => x.UserModules);
            Query.Where(x => x.UserModules.Select(x => x.UserId).Contains(userId));
            Query.Include(x => x.Labs);
            Query.Include(x => x.UserModules).ThenInclude(x => x.User);
            Query.Take(1);
        }
    }
}
