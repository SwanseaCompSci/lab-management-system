using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.ModuleCommands
{
    public class TestsUpdateCommandValidator
    {
        private Update.CommandValidator Validator { get; set; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new Update.Command()
            {
                Id = Guid.NewGuid(),
                Name = "Programming 2",
                Code = "CS-115",
                Level = "Year1",
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new Update.Command()
            {
                Id = Guid.Empty,
                Name = string.Empty,
                Code = string.Empty,
                Level = string.Empty,
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(5);
            result.Errors[0].ErrorMessage.Should().Be("'Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Name' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Code' must not be empty.");
            result.Errors[3].ErrorMessage.Should().Be("'Level' has a range of values which does not include ''.");
            result.Errors[4].ErrorMessage.Should().Be("'Level' must not be empty.");
        }
    }
}
