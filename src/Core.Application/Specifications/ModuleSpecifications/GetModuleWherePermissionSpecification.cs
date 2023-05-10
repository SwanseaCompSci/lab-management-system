using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetModuleWherePermissionSpecification : Specification<Module>, ISingleResultSpecification<Module>
    {
        public GetModuleWherePermissionSpecification(Guid moduleId, Guid userId)
        {
            Query.Where(x => x.Id.Equals(moduleId));
            Query.Include(x => x.UserModules);
            Query.Where(x => x.UserModules.Select(x => x.UserId).Contains(userId));
            Query.Take(1);
        }
    }
}
