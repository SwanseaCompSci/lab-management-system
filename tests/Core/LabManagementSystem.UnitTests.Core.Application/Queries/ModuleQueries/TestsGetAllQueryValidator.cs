using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.ModuleQueries
{
    public class TestsGetAllQueryValidator
    {
        private GetAll.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            var query = new GetAll.Query();

            Validator.Validate(query).IsValid.Should().BeTrue();
        }
    }
}
