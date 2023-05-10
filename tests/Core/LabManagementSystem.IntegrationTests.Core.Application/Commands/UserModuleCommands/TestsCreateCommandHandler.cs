using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserModuleCommands
{
    public sealed class TestsCreateCommandHandler : TestBase
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

            var command = new Create.Command()
            {
                UserId = user.Id,
                ModuleId = module.Id,
                Role = ModuleRole.TeachingAssistant.ToString(),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.UserId.Should().Be(user.Id);
            response.Resource.ModuleId.Should().Be(module.Id);
            response.Resource.Role.Should().Be(command.Role);
        }
    }
}
