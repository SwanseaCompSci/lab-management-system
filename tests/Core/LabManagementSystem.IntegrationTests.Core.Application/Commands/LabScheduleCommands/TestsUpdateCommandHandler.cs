using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.LabScheduleCommands
{
    public sealed class TestsUpdateCommandHandler : TestBase
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

            var labSchedule = new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 05, 14, 00, 00), end: new DateTime(2023, 02, 05, 16, 00, 00));
            await Testing.AddAsync(entity: labSchedule);

            var command = new Update.Command()
            {
                Id = labSchedule.Id,
                LabId = labSchedule.LabId,
                Date = new DateTime(2023, 02, 06),
                Start = new TimeSpan(15, 00, 00),
                End = new TimeSpan(17, 00, 00),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource.Id.Should().Be(command.Id);
            response.Resource.LabId.Should().Be(command.LabId);
            response.Resource.Start.Should().Be(new DateTime(2023, 02, 06, 15, 00, 00));
            response.Resource.End.Should().Be(new DateTime(2023, 02, 06, 17, 00, 00));
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Update.Command()
            {
                Id = Guid.NewGuid(),
                LabId = Guid.NewGuid(),
                Date = new DateTime(2023, 02, 06),
                Start = new TimeSpan(14, 00, 00),
                End = new TimeSpan(16, 00, 00),
            };

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'LabSchedule' ({command.Id}) was not found.");
        }
    }
}
