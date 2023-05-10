using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserLabQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.UserLabQueries
{
    public class TestsGetByLabIdQueryValidator
    {
        private GetByLabId.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new GetByLabId.Query(labId: Guid.NewGuid());

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new GetByLabId.Query(labId: Guid.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("'Lab Id' must not be empty.");
        }
    }
}
