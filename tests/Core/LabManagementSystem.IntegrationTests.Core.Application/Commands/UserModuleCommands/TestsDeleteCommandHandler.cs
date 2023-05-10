using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserModuleCommands
{
    public sealed class TestsDeleteCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("71e9d5be-edf1-4430-a7da-06b33203cd71"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var userModule = new UserModule(userId: user.Id, moduleId: module.Id, role: ModuleRole.LabCoordinator);
            await Testing.AddAsync(entity: userModule);

            var command = new Delete.Command(userId: userModule.UserId, moduleId: userModule.ModuleId);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.UserId.Should().Be(userModule.UserId);
            response.Resource.ModuleId.Should().Be(userModule.ModuleId);
            response.Resource.Role.Should().Be(userModule.Role.ToString());
        }

        [Test]
        public async Task Handle_Command_Non_Existing_Entity()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Delete.Command(userId: Guid.NewGuid(), moduleId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
