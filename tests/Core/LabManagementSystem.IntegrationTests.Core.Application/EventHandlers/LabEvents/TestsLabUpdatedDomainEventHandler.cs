using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Events.LabEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.EventHandlers.LabEvents
{
    public sealed class TestsLabUpdatedDomainEventHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());
            Testing.RunOnDateTime(dateTime: new DateTime(2023, 01, 01));

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var oldLab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            var newLab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00), minNumberOfStaff: 5, maxNumberOfStaff: 6);
            newLab.SetPrivatePropertyValue("Id", oldLab.Id);
            await Testing.AddAsync(entity: newLab);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: oldLab.Id, start: new DateTime(2023, 02, 10, 14, 00, 00), end: new DateTime(2023, 02, 10, 15, 00, 00)),
                new LabSchedule(labId: oldLab.Id, start: new DateTime(2023, 02, 17, 14, 00, 00), end: new DateTime(2023, 02, 17, 15, 00, 00)),
                new LabSchedule(labId: oldLab.Id, start: new DateTime(2023, 02, 24, 14, 00, 00), end: new DateTime(2023, 02, 24, 15, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var domainEvent = new LabUpdatedDomainEvent(oldLab: oldLab, newLab: newLab);
            var notification = new DomainEventNotification<LabUpdatedDomainEvent>(domainEvent: domainEvent);

            // Act
            await Testing.PublishAsync(notification);

            // Assert
            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            for (int i = 0; i < labSchedules.Count; i++)
            {
                var start = new DateTime(2023, 02, 10, 15, 00, 00).AddDays((i * 7) - 1);
                var end = new DateTime(2023, 02, 10, 16, 00, 00).AddDays((i * 7) - 1);

                applicationDbContext.LabSchedules.Any(x => x.Start == start && x.End == end).Should().BeTrue();
            }
        }
    }
}
