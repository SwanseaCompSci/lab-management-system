using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserCommands
{
    public sealed class TestsDeleteCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("357c0c86-32f2-4f5e-a8fd-79692b0a20e2"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 40);
            await Testing.AddAsync(entity: user);

            var command = new Delete.Command(userId: user.Id);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.FirstName.Should().Be(user.FirstName);
            response.Resource.Surname.Should().Be(user.Surname);
            response.Resource.AchievedLevel.Should().Be(user.AchievedLevel.ToString());
            response.Resource.MaxWeeklyWorkHours.Should().Be(user.MaxWeeklyWorkHours);
            response.Resource.QuestionnaireToken.Should().Be(user.QuestionnaireToken);
        }

        [Test]
        public async Task Handle_Command_Non_Existing_Entity()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Delete.Command(userId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
