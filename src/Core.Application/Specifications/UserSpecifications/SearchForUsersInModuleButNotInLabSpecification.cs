using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    // TODO: Add docs comments
    public sealed class SearchForUsersInModuleButNotInLabSpecification : Specification<User>
    {
        public SearchForUsersInModuleButNotInLabSpecification(Guid moduleId, Guid labId, string searchExpression)
        {
            Query.Include(x => x.UserModules);
            Query.Where(x => x.UserModules.Select(um => um.ModuleId).Contains(moduleId));

            Query.Include(x => x.UserLabs);
            Query.Where(x => !x.UserLabs.Select(x => x.LabId).Contains(labId));

            Query.Search(x => x.FirstName, "%" + searchExpression + "%");
            Query.Search(x => x.Surname, "%" + searchExpression + "%");
        }
    }
}
