using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserLabsWhereModuleSpecification : Specification<UserLab>
    {
        public GetUserLabsWhereModuleSpecification(Guid userId, Guid moduleId)
        {
            Query.Where(x => x.UserId.Equals(userId));
            Query.Include(x => x.Lab);
            Query.Where(x => x.Lab.ModuleId.Equals(moduleId));
        }
    }
}
