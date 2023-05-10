using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.LabScheduleCommands
{
    public class TestsCreateCommandValidator
    {
        private IDateTimeService DateTimeService { get; set; } = null!;
        private Create.CommandValidator Validator { get; set; } = null!;

        [OneTimeSetUp]
        public void Initialize()
        {
            DateTimeService = Mocks.GetDateTimeServiceMock();
            Validator = new(dateTimeService: DateTimeService);
        }

        [Test]
        public void Command_Valid()
        {
            var command = new Create.Command()
            {
                LabId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Start = new TimeOnly(13, 00).ToTimeSpan(),
                End = new TimeOnly(15, 00).ToTimeSpan(),
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid_Empty_Or_Null_Properties()
        {
            var command = new Create.Command()
            {
                LabId = Guid.Empty,
                Date = null,
                Start = null,
                End = null,
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(4);
            result.Errors[0].ErrorMessage.Should().Be("'Lab Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Date' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Start' must not be empty.");
            result.Errors[3].ErrorMessage.Should().Be("'End' must not be empty.");
        }

        [Test]
        public void Command_Invalid_Properties_Out_of_Range()
        {
            var command = new Create.Command()
            {
                LabId = Guid.NewGuid(),
                Date = new DateTime(2000, 01, 01),
                Start = new TimeOnly(12, 00).ToTimeSpan(),
                End = new TimeOnly(10, 00).ToTimeSpan(),
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(2);
            result.Errors[0].ErrorMessage.Should().Be("'Date' must be greater than or equal to '01/01/2020 00:00:00'.");
            result.Errors[1].ErrorMessage.Should().Be("'End' must be greater than '12:00:00'.");
        }
    }
}
