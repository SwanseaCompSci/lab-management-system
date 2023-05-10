using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.ModuleQueries
{
    public class TestsGetQueryValidator
    {
        private Get.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new Get.Query(moduleId: Guid.NewGuid());

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new Get.Query(moduleId: Guid.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("'Module Id' must not be empty.");
        }
    }
}
