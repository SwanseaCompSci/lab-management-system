using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserCommands
{
    public sealed class TestsUpdateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("31a2684e-7562-4168-bab1-38f79cb65c1d"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var command = new Update.Command()
            {
                Id = user.Id,
                FirstName = "Anna",
                Surname = "Hunt",
                AchievedLevel = Level.PhD.ToString(),
                MaxWeeklyWorkHours = 48,
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

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Update.Command()
            {
                Id = Guid.NewGuid(),
                FirstName = "Mike",
                Surname = "Ross",
                AchievedLevel = Level.Year3.ToString(),
                MaxWeeklyWorkHours = 40,
            };

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'User' ({command.Id}) was not found.");
        }
    }
}
