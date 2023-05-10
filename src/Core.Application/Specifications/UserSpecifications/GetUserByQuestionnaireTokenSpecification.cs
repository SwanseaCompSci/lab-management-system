using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    // TODO: Add docs comments
    public sealed class GetUserByQuestionnaireTokenSpecification : Specification<User>, ISingleResultSpecification<User>
    {
        public GetUserByQuestionnaireTokenSpecification(Guid token)
        {
            Query.Where(x => x.QuestionnaireToken.Equals(token));
            Query.Take(1);
        }
    }
}
