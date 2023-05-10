using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModulePreferenceCommands
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

            var user = new User(id: Guid.Parse("a7951a6c-e552-433b-a2ea-8ab0b9e48f3c"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var modulePreference = new ModulePreference(userId: user.Id, moduleId: module.Id);
            await Testing.AddAsync(entity: modulePreference);

            var command = new Update.Command(userId: user.Id, moduleId: module.Id, status: Status.Accepted.ToString());

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.UserId.Should().Be(command.UserId);
            response.Resource.ModuleId.Should().Be(command.ModuleId);
            response.Resource.Status.Should().Be(command.Status);
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Update.Command(userId: Guid.Parse("81daa0a0-ea8d-40c5-b79b-51e3241de003"),
                                             moduleId: Guid.Parse("5168dd64-c4d3-437f-95ce-ba6cd7b4340b"),
                                             status: Status.Declined.ToString());

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'ModulePreference' ({command.UserId}, {command.ModuleId}) was not found.");
        }
    }
}
