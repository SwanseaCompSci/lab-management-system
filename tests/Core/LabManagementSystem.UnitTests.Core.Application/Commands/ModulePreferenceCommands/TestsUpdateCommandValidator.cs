using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.ModulePreferenceCommands
{
    public sealed class TestsUpdateCommandValidator
    {
        private Update.CommandValidator Validator { get; set; } = new();

        [Test]
        public void Command_Valid()
        {
            // Arrange
            var command = new Update.Command(userId: Guid.Parse("fd4e2ec3-3a10-42b7-ba03-ac723c478826"),
                                             moduleId: Guid.Parse("884d2e78-6ad2-4918-95e5-1749d1b03d81"),
                                             status: Status.Accepted.ToString());

            // Act & Assert
            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            // Arrange
            var command = new Update.Command(userId: Guid.Empty, moduleId: Guid.Empty, status: string.Empty);

            // Act
            var result = Validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(4);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Module Id' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Status' has a range of values which does not include ''.");
            result.Errors[3].ErrorMessage.Should().Be("'Status' must not be empty.");
        }
    }
}
