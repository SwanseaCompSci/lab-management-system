using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserModuleEvents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.EventHandlers.UserModuleEvents
{
    public sealed class TestsUserRemovedFromModuleDomainEventHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("30744df6-6616-47c0-a093-02d9f630e99d"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: user.Id, labId: labs[0].Id),
                new UserLab(userId: user.Id, labId: labs[1].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var domainEvent = new UserRemovedFromModuleDomainEvent(userId: user.Id, moduleId: module.Id);
            var notification = new DomainEventNotification<UserRemovedFromModuleDomainEvent>(domainEvent: domainEvent);

            // Act
            await Testing.PublishAsync(notification);

            // Assert
            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            applicationDbContext.UserLabs.Should().BeEmpty();
        }
    }
}
