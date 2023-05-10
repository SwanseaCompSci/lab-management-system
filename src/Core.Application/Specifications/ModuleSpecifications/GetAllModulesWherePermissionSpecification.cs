using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetAllModulesWherePermissionSpecification : Specification<Module>
    {
        public GetAllModulesWherePermissionSpecification(Guid userId)
        {
            Query.Include(x => x.UserModules);
            Query.Where(x => x.UserModules.Select(x => x.UserId).Contains(userId));
        }
    }
}
