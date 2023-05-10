using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserLabCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.UserLabCommands
{
    public class TestsCreateCommandValidator
    {
        private Create.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Create.Command(userId: Guid.NewGuid(),
                                             labId: Guid.NewGuid());

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Create.Command(userId: Guid.Empty,
                                             labId: Guid.Empty);

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(2);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Lab Id' must not be empty.");
        }
    }
}
