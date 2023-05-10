using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.LabCommands
{
    public class TestsDeleteCommandValidator
    {
        private Delete.CommandValidator Validator { get; set; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Delete.Command(labId: Guid.NewGuid());

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Delete.Command(labId: Guid.Empty);

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("'Lab Id' must not be empty.");
        }
    }
}
