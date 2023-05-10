using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.UserLabEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.EventHandlers.UserLabEvents
{
    public sealed class TestsUserAddedToLabDomainEventHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());
            Testing.RunOnDateTime(dateTime: new DateTime(2023, 02, 01));

            var user = new User(id: Guid.Parse("30744df6-6616-47c0-a093-02d9f630e99d"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            await Testing.AddAsync(entity: lab);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 06, 12, 00, 00), end: new DateTime(2023, 02, 06, 13, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 13, 12, 00, 00), end: new DateTime(2023, 02, 13, 13, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 20, 12, 00, 00), end: new DateTime(2023, 02, 20, 13, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 27, 12, 00, 00), end: new DateTime(2023, 02, 27, 13, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var domainEvent = new UserAddedToLabDomainEvent(userId: user.Id, labId: lab.Id);
            var notification = new DomainEventNotification<UserAddedToLabDomainEvent>(domainEvent: domainEvent);

            // Act
            await Testing.PublishAsync(notification);

            // Assert
            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            applicationDbContext.UserLabSchedules.Should().HaveCount(4);

            var resultTimeAvailability = applicationDbContext.TimeAvailabilities
                .FirstOrDefault(x => x.UserId == user.Id && x.StartTime == new TimeOnly(12, 00) && x.EndTime == new TimeOnly(13, 00))
                ?? throw new NullReferenceException();
            resultTimeAvailability.IsAllocated.Should().BeTrue();
        }
    }
}
