using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;
using System;
using System.Collections.Generic;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.TimeAvailabilityCommands
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
                Token = Guid.NewGuid(),
                TimeAvailabilities = new List<CreateRange.TimeAvailabilityCommandModel>()
                {
                    new CreateRange.TimeAvailabilityCommandModel()
                    {
                        Day = "Monday",
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(12, 00),
                    }
                },
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid()
        {
            var command = new CreateRange.Command()
            {
                UserId = Guid.Empty,
                Token = Guid.Empty,
                TimeAvailabilities = new List<CreateRange.TimeAvailabilityCommandModel>(),
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(3);
            result.Errors[0].ErrorMessage.Should().Be("'User Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Token' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Time Availabilities' must not be empty.");
        }
    }
}
