using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserCommands
{
    public sealed class TestsGenerateQuestionnaireTokenCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("3737482d-4233-4fa8-b0a6-06ce97971f88"), firstName: "Mike", surname: "Ross", achievedLevel: LabManagementSystem.Core.Domain.Enums.Level.Year3, maxWeeklyWorkHours: 40);
            await Testing.AddAsync(entity: user);

            var command = new GenerateQuestionnaireToken.Command(userId: user.Id);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Id.Should().Be(user.Id);
            response.Resource.FirstName.Should().Be(user.FirstName);
            response.Resource.Surname.Should().Be(user.Surname);
            response.Resource.AchievedLevel.Should().Be(user.AchievedLevel.ToString());
            response.Resource.MaxWeeklyWorkHours.Should().Be(user.MaxWeeklyWorkHours);
            response.Resource.QuestionnaireToken.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new GenerateQuestionnaireToken.Command(userId: Guid.Parse("a7a2e751-3e60-487d-9808-4ccc6033f6f9"));

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'User' ({command.UserId}) was not found.");
        }
    }
}
