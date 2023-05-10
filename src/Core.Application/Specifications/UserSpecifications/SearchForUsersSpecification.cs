using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    // TODO: Add docs comments
    public sealed class SearchForUsersSpecification : Specification<User>
    {
        public SearchForUsersSpecification(string searchExpression)
        {
            Query.Search(x => x.FirstName, "%" + searchExpression + "%");
            Query.Search(x => x.Surname, "%" + searchExpression + "%");
        }
    }
}
