using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.UserModuleCommands
{
    public class TestsCreateCommandValidator
    {
        private Create.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Create.Command()
            {
                UserId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Role = "ModuleCoordinator",
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Create.Command()
            {
                UserId = Guid.Empty,
                ModuleId = Guid.Empty,
                Role = string.Empty,
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(4);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Module Id' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Role' has a range of values which does not include ''.");
            result.Errors[3].ErrorMessage.Should().Be("'Role' must not be empty.");
        }
    }
}
