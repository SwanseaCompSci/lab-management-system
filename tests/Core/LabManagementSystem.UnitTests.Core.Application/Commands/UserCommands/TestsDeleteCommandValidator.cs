using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.UserCommands
{
    public class TestsDeleteCommandValidator
    {
        private Delete.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Validate()
        {
            var command = new Delete.Command(userId: Guid.NewGuid());

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Delete.Command(userId: Guid.Empty);

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
        }
    }
}
