using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.LabScheduleCommands
{
    public sealed class TestsCreateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            await Testing.AddAsync(entity: lab);

            var command = new Create.Command()
            {
                LabId = lab.Id,
                Start = new TimeOnly(14, 00, 00).ToTimeSpan(),
                End = new TimeOnly(16, 00, 00).ToTimeSpan(),
                Date = new DateTime(2023, 02, 06),
            };

            // Act
            var result = await Testing.SendAsync(command);

            // Assert
            result.Resource.Should().NotBeNull();
            result.Resource.Id.Should().NotBeEmpty();
            result.Resource.LabId.Should().Be(command.LabId);
            result.Resource.Start.Should().Be(new DateTime(2023, 02, 06, 14, 00, 00));
            result.Resource.End.Should().Be(new DateTime(2023, 02, 06, 16, 00, 00));
        }
    }
}
