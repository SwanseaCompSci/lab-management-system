using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabScheduleEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.EventHandlers.LabScheduleEvents
{
    public sealed class TestsLabScheduleCreatedDomainEventHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),

                new User(id: Guid.Parse("d6148930-d184-4d3f-9a60-ae5fd9e24325"), firstName: "Adam", surname: "Vega", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("cc5d36af-c7f0-4d0b-b3a4-9d76075ac892"), firstName: "Josh", surname: "Hunt", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 3, maxNumberOfStaff: 5);
            await Testing.AddAsync(entity: lab);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: lab.Id),
                new UserLab(userId: users[1].Id, labId: lab.Id),
                new UserLab(userId: users[2].Id, labId: lab.Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedule = new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 10, 12, 00, 00), end: new DateTime(2023, 02, 10, 13, 00, 00));
            await Testing.AddAsync(entity: labSchedule);

            var domainEvent = new LabScheduleCreatedDomainEvent(labSchedule: labSchedule);
            var notification = new DomainEventNotification<LabScheduleCreatedDomainEvent>(domainEvent: domainEvent);

            // Act
            await Testing.PublishAsync(notification);

            // Assert
            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();
            applicationDbContext.UserLabSchedules.Should().HaveCount(3);

            for (int i = 0; i < 3; i++)
            {
                applicationDbContext.UserLabSchedules.Any(x => x.UserId == users[i].Id && x.LabScheduleId == labSchedule.Id).Should().BeTrue();
            }
        }
    }
}
