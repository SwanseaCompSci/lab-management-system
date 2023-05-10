using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserCommands
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
                Id = Guid.Parse("fafb31fb-7749-4c9f-9e75-393eb916fee4"),
                FirstName = "Mike",
                Surname = "Ross",
                AchievedLevel = Level.Year1.ToString(),
                MaxWeeklyWorkHours = 40,
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Id.Should().Be(command.Id);
            response.Resource.FirstName.Should().Be(command.FirstName);
            response.Resource.Surname.Should().Be(command.Surname);
            response.Resource.AchievedLevel.Should().Be(command.AchievedLevel);
            response.Resource.MaxWeeklyWorkHours.Should().Be(command.MaxWeeklyWorkHours);
            response.Resource.QuestionnaireToken.Should().BeNull();
        }
    }
}
