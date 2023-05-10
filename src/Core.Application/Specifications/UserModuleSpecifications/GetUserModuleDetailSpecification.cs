using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserModuleSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserModuleDetailSpecification : Specification<UserModule>, ISingleResultSpecification<UserModule>
    {
        public GetUserModuleDetailSpecification(Guid userId, Guid moduleId)
        {
            Query.Where(x => x.UserId == userId && x.ModuleId == moduleId);
            Query.Include(x => x.User);
            Query.Include(x => x.Module);
        }
    }
}
