using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.TimeAvailabilityCommands
{
    public class TestsDeleteAllCommandValidator
    {
        private DeleteAll.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var query = new DeleteAll.Command();

            Validator.Validate(query).IsValid.Should().BeTrue();
        }
    }
}
