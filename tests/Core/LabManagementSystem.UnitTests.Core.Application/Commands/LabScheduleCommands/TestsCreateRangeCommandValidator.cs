using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Commands.LabScheduleCommands
{
    public class TestsCreateRangeCommandValidator
    {
        private IDateTimeService DateTimeService { get; set; } = null!;
        private CreateRange.CommandValidator Validator { get; set; } = null!;

        [OneTimeSetUp]
        public void Initialize()
        {
            DateTimeService = Mocks.GetDateTimeServiceMock();
            Validator = new(dateTimeService: DateTimeService);
        }

        [Test]
        public void Command_Valid()
        {
            var command = new CreateRange.Command()
            {
                LabId = Guid.NewGuid(),
                StartDate = DateTimeService.Today,
                Start = new TimeOnly(14, 00).ToTimeSpan(),
                End = new TimeOnly(16, 00).ToTimeSpan(),
                NumberOfOccurrences = 5,
            };

            Validator.Validate(command).IsValid.Should().BeTrue();
        }

        [Test]
        public void Command_Invalid_Empty_Or_Null_Properties()
        {
            var command = new CreateRange.Command()
            {
                LabId = Guid.Empty,
                StartDate = null,
                NumberOfOccurrences = 0,
                Start = null,
                End = null,
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(5);
            result.Errors[0].ErrorMessage.Should().Be("'Lab Id' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Start Date' must not be empty.");
            result.Errors[2].ErrorMessage.Should().Be("'Number Of Occurrences' must be greater than or equal to '1'.");
            result.Errors[3].ErrorMessage.Should().Be("'Start' must not be empty.");
            result.Errors[4].ErrorMessage.Should().Be("'End' must not be empty.");
        }

        [Test]
        public void Command_Invalid_Properties_Out_of_Range()
        {
            var command = new CreateRange.Command()
            {
                LabId = Guid.NewGuid(),
                StartDate = new DateTime(2000, 1, 1),
                NumberOfOccurrences = 0,
                Start = new TimeOnly(12, 00).ToTimeSpan(),
                End = new TimeOnly(10, 00).ToTimeSpan(),
            };

            var result = Validator.Validate(command);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(3);
            result.Errors[0].ErrorMessage.Should().Be("'Start Date' must be greater than or equal to '01/01/2020 00:00:00'.");
            result.Errors[1].ErrorMessage.Should().Be("'Number Of Occurrences' must be greater than or equal to '1'.");
            result.Errors[2].ErrorMessage.Should().Be("'End' must be greater than '12:00:00'.");
        }
    }
}
