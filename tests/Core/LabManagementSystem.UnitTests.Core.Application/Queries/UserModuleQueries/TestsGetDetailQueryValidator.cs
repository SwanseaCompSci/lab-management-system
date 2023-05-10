using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserModuleQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.UserModuleQueries
{
    public class TestsGetDetailQueryValidator
    {
        private GetDetail.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new GetDetail.Query(userId: Guid.NewGuid(),
                                            moduleId: Guid.NewGuid());

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new GetDetail.Query(userId: Guid.Empty,
                                            moduleId: Guid.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(2);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Module Id' must not be empty.");
        }
    }
}
