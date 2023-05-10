using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserEvents;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.EventHandlers.UserEvents
{
    public sealed class TestsUserTimeAvailabilitySubmittedDomainEventHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("ea724c08-2387-4e17-afb6-7b0fd43fc836"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20)
            {
                QuestionnaireToken = Guid.Parse("6ca5f929-6c3e-4a17-a27e-f35e406c3bae"),
            };
            await Testing.AddAsync(entity: user);

            var domainEvent = new UserTimeAvailabilitySubmittedDomainEvent(userId: user.Id);
            var notification = new DomainEventNotification<UserTimeAvailabilitySubmittedDomainEvent>(domainEvent: domainEvent);

            // Act
            await Testing.PublishAsync(notification);

            // Assert
            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();
            var result = applicationDbContext.Users.FirstOrDefault(x => x.Id == user.Id) ?? throw new NullReferenceException();
            result.QuestionnaireToken.Should().BeNull();
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var domainEvent = new UserTimeAvailabilitySubmittedDomainEvent(userId: Guid.NewGuid());
            var notification = new DomainEventNotification<UserTimeAvailabilitySubmittedDomainEvent>(domainEvent: domainEvent);

            // Act & Assert
            await FluentActions.Invoking(() => Testing.PublishAsync(notification))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'User' ({notification.Event.UserId}) was not found.");
        }
    }
}
