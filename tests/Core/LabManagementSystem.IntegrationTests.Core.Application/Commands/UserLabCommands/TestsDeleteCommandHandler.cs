using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserLabCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserLabCommands
{
    public sealed class TestsDeleteCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("13c91cae-bf26-4eea-91ff-9399eb1a03f0"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 6, maxNumberOfStaff: 8);
            await Testing.AddAsync(entity: lab);

            var userLab = new UserLab(userId: user.Id, labId: lab.Id);
            await Testing.AddAsync(entity: userLab);

            var command = new Delete.Command(userId: userLab.UserId, labId: userLab.LabId);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.UserId.Should().Be(userLab.UserId);
            response.Resource.LabId.Should().Be(userLab.LabId);
        }

        [Test]
        public async Task Handle_Command_Non_Existing_Entity()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Delete.Command(userId: Guid.NewGuid(), labId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
