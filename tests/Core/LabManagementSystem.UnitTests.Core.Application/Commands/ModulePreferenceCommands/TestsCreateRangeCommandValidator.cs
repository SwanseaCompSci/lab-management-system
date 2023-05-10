using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using System;
using System.Collections.Generic;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.ModulePreferenceCommands
{
    public class TestsCreateRangeCommandValidator
    {
        private CreateRange.CommandValidator Validator { get; } = new();

        [Test]
        public void Command_Valid()
        {
            var command = new CreateRange.Command()
            {
                UserId = Guid.NewGuid(),
                ModuleIds = new List<Guid>()
                {
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                },
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid_Empty_Properties()
        {
            var command = new CreateRange.Command()
            {
                UserId = Guid.Empty,
                ModuleIds = new List<Guid>(),
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(2);

            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Module Ids' must not be empty.");
        }
    }
}
