using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.LabCommands
{
    public sealed class TestsDeleteCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(12, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            await Testing.AddAsync(entity: lab);

            var command = new Delete.Command(labId: lab.Id);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(lab.Id);
            response.Resource.ModuleId.Should().Be(lab.ModuleId);
            response.Resource.Name.Should().Be(lab.Name);
            response.Resource.Day.Should().Be(lab.Day);
            response.Resource.StartTime.Should().Be(lab.StartTime);
            response.Resource.EndTime.Should().Be(lab.EndTime);
            response.Resource.MinNumberOfStaff.Should().Be(lab.MinNumberOfStaff);
            response.Resource.MaxNumberOfStaff.Should().Be(lab.MaxNumberOfStaff);
        }

        [Test]
        public async Task Handle_Command_Non_Existing_Entity()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Delete.Command(labId: Guid.Parse("9c8dbb3c-a80f-42c1-bf57-d68b2375ed69"));

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
