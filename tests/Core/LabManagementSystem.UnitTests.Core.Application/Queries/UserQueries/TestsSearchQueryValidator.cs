using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.UserQueries
{
    public class TestsSearchQueryValidator
    {
        private Search.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new Search.Query(searchExpression: "Ella");

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new Search.Query(searchExpression: string.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("'Search Expression' must not be empty.");
        }
    }
}
