using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.UserQueries
{
    public class TestsSearchInModuleButNotInLabQueryValidator
    {
        private SearchInModuleButNotInLab.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new SearchInModuleButNotInLab.Query(moduleId: Guid.NewGuid(),
                                                            labId: Guid.NewGuid(),
                                                            searchExpression: "Ella");

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new SearchInModuleButNotInLab.Query(moduleId: Guid.Empty,
                                                            labId: Guid.Empty,
                                                            searchExpression: string.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(3);
            result.Errors[0].ErrorMessage.Should().Be("'Module Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Lab Id' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Search Expression' must not be empty.");
        }
    }
}
