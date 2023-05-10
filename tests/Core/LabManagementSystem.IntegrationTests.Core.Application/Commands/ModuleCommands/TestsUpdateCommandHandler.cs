using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModuleCommands
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

            var command = new Update.Command()
            {
                Id = module.Id,
                Name = "Programming 2",
                Code = "CS-220",
                Level = Level.Year2.ToString(),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Id.Should().Be(command.Id);
            response.Resource.Name.Should().Be(command.Name);
            response.Resource.Code.Should().Be(command.Code);
            response.Resource.Level.Should().Be(command.Level);
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Update.Command()
            {
                Id = Guid.NewGuid(),
                Name = "Concurrency",
                Code = "CS-210",
                Level = Level.Year2.ToString(),
            };

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'Module' ({command.Id}) was not found.");
        }
    }
}
