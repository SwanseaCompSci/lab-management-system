using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.UserModuleCommands
{
    public class TestsDeleteCommandValidator
    {
        private Delete.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Delete.Command(userId: Guid.NewGuid(),
                                             moduleId: Guid.NewGuid());

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Delete.Command(userId: Guid.Empty,
                                             moduleId: Guid.Empty);

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(2);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Module Id' must not be empty.");
        }
    }
}
