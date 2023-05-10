using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabScheduleQueries;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.LabScheduleQueries
{
    public class TestsGetQueryValidator
    {
        private Get.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new Get.Query(labScheduleId: Guid.NewGuid());

            Validator.Validate(query).IsValid.Should().BeTrue();
        }

        [Test]
        public void Query_Invalid()
        {
            var query = new Get.Query(labScheduleId: Guid.Empty);

            var result = Validator.Validate(query);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("'Lab Schedule Id' must not be empty.");
        }
    }
}
