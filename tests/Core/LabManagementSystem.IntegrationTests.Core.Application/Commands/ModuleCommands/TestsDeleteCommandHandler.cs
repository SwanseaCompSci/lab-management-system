using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModuleCommands
{
    public sealed class TestsDeleteCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 2", code: "CS-115", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var command = new Delete.Command(moduleId: module.Id);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(module.Id);
            response.Resource.Name.Should().Be(module.Name);
            response.Resource.Code.Should().Be(module.Code);
            response.Resource.Level.Should().Be(module.Level.ToString());
        }

        [Test]
        public async Task Handle_Command_Non_Existing_Entity()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Delete.Command(moduleId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
