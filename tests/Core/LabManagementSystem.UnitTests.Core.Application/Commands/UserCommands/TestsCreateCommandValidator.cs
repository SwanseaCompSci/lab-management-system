using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.UserCommands
{
    public class TestsCreateCommandValidator
    {
        private Create.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Create.Command()
            {
                Id = Guid.NewGuid(),
                FirstName = "Marie",
                Surname = "Kovárníková",
                AchievedLevel = "Year1",
                MaxWeeklyWorkHours = 10,
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Create.Command()
            {
                Id = Guid.Empty,
                FirstName = string.Empty,
                Surname = string.Empty,
                AchievedLevel = string.Empty,
                MaxWeeklyWorkHours = 50,
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(6);
            result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'First Name' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Surname' must not be empty.");
            result.Errors[3].ErrorMessage.Should().Be("'Achieved Level' has a range of values which does not include ''.");
            result.Errors[4].ErrorMessage.Should().Be("'Achieved Level' must not be empty.");
            result.Errors[5].ErrorMessage.Should().Be("'Max Weekly Work Hours' must be less than or equal to '48'.");
        }
    }
}
