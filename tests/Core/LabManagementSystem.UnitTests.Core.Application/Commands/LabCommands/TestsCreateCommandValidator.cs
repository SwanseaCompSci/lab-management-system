using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.LabCommands
{
    public class TestsCreateCommandValidator
    {
        private Create.CommandValidator Validator { get; set; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Create.Command()
            {
                ModuleId = Guid.NewGuid(),
                Name = "Unit Test Lab",
                Day = "Monday",
                StartTime = new TimeOnly(10, 00).ToTimeSpan(),
                EndTime = new TimeOnly(12, 00).ToTimeSpan(),
                MinNumberOfStaff = 2,
                MaxNumberOfStaff = 2,
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Create.Command()
            {
                ModuleId = Guid.Empty,
                Name = string.Empty,
                Day = string.Empty,
                StartTime = null,
                EndTime = null,
                MinNumberOfStaff = 0,
                MaxNumberOfStaff = 0,
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(9);
            result.Errors[0].ErrorMessage.Should().Be("'Module Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Name' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Day' has a range of values which does not include ''.");
            result.Errors[3].ErrorMessage.Should().Be("'Day' must not be empty.");
            result.Errors[4].ErrorMessage.Should().Be("'Start Time' must not be empty.");
            result.Errors[5].ErrorMessage.Should().Be("'End Time' must not be empty.");
            result.Errors[6].ErrorMessage.Should().Be("'Min Number Of Staff' must be greater than or equal to '1'.");
            result.Errors[7].ErrorMessage.Should().Be("'Min Number Of Staff' must not be empty.");
            result.Errors[8].ErrorMessage.Should().Be("'Max Number Of Staff' must not be empty.");
        }
    }
}
