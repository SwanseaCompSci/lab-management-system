using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModuleCommands
{
    public sealed class TestsDeleteAllCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(2);
            foreach (var item in modules)
            {
                response.Resource!.Any(x => x.Id == item.Id).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Command_No_Resources_To_Delete()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
