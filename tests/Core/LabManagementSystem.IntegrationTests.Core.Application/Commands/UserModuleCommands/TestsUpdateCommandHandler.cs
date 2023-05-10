using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserModuleCommands
{
    public sealed class TestsUpdateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("c9747975-cbaa-4cb9-a9de-97ef98cb6dab"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var userModule = new UserModule(userId: user.Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant);
            await Testing.AddAsync(entity: userModule);

            var command = new Update.Command()
            {
                UserId = user.Id,
                ModuleId = module.Id,
                Role = ModuleRole.LabCoordinator.ToString(),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.UserId.Should().Be(user.Id);
            response.Resource.ModuleId.Should().Be(module.Id);
            response.Resource.Role.Should().Be(command.Role);
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Update.Command()
            {
                UserId = Guid.NewGuid(),
                ModuleId = Guid.NewGuid(),
                Role = ModuleRole.ModuleCoordinator.ToString(),
            };

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'UserModule' ({command.UserId}, {command.ModuleId}) was not found.");
        }
    }
}
