using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModuleCommands
{
    public sealed class TestsCreateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Create.Command()
            {
                Name = "Programming 1",
                Code = "CS-110",
                Level = Level.Year1.ToString(),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Id.Should().NotBeEmpty();
            response.Resource.Name.Should().Be(command.Name);
            response.Resource.Code.Should().Be(command.Code);
            response.Resource.Level.Should().Be(command.Level);
        }
    }
}
