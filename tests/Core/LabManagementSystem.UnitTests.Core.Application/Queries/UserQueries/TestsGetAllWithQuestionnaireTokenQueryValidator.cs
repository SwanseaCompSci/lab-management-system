using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.UserQueries
{
    public class TestsGetAllWithQuestionnaireTokenQueryValidator
    {
        private GetAllWithQuestionnaireToken.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new GetAllWithQuestionnaireToken.Query();

            Validator.Validate(query).IsValid.Should().BeTrue();
        }
    }
}
