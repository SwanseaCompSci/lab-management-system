using Ardalis.Specification;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications
{
    /// <summary>
    /// Query logic to retrieve all <see cref="User"/> entities with questionnaire token from the database.
    /// </summary>
    public sealed class GetAllUsersWithQuestionnaireTokenSpecification : Specification<User>
    {
        public GetAllUsersWithQuestionnaireTokenSpecification()
        {
            Query.Where(x => x.QuestionnaireToken != null);
        }
    }
}
