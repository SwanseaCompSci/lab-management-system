using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModulePreferenceQueries;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Queries.ModulePreferenceQueries
{
    public class TestsGetAllQueryValidator
    {
        private GetAll.QueryValidator Validator { get; } = new();

        [Test]
        public void Query_Valid()
        {
            Validator.Validate(new GetAll.Query()).IsValid.Should().BeTrue();
        }
    }
}
