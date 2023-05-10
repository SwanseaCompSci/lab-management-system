using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.ModuleQueries
{
    public class TestsGetAllBelowOrEqualLevelQueryValidator
    {
        private GetAllBelowOrEqualLevel.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new GetAllBelowOrEqualLevel.Query(level: "Year3");

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new GetAllBelowOrEqualLevel.Query(level: string.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(2);
            result.Errors[0].ErrorMessage.Should().Be("'Level' has a range of values which does not include ''.");
            result.Errors[1].ErrorMessage.Should().Be("'Level' must not be empty.");
        }
    }
}
