using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.AllocationQueries;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.AllocationQueries
{
    public sealed class TestsGetLabAllocationsQueryValidator
    {
        private GetLabAllocations.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new GetLabAllocations.Query();

            Validator.Validate(query).IsValid.Should().BeTrue();
        }
    }
}
