using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabScheduleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.LabScheduleSpecifications
{
    public sealed class TestsGetLabSchedulesWhereLabFromDateTimeSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Test Module", code: "CS-xxx", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Test Lab 1", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: module.Id, name: "Test Lab 2", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 06, 14, 00, 00), end: new DateTime(2023, 02, 06, 15, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 13, 14, 00, 00), end: new DateTime(2023, 02, 13, 15, 00, 00)),

                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 06, 12, 00, 00), end: new DateTime(2023, 02, 06, 13, 00, 00)),
                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 13, 12, 00, 00), end: new DateTime(2023, 02, 13, 13, 00, 00)),
                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 07, 20, 12, 00, 00), end: new DateTime(2023, 02, 20, 13, 00, 00)),
                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 07, 27, 12, 00, 00), end: new DateTime(2023, 02, 27, 13, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var dateTime = new DateTime(2023, 02, 15);
            var specification = new GetLabSchedulesWhereLabFromDateTimeSpecification(labId: labs[1].Id, dateTime: dateTime);

            // Act
            var result = applicationDbContext.LabSchedules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Any(x => x.Start < dateTime || x.End < dateTime).Should().BeFalse();
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabSchedulesWhereLabFromDateTimeSpecification(labId: Guid.Parse("99ef572f-c879-484f-8f66-4cdb98dd629f"),
                                                                                     dateTime: new DateTime(2023, 02, 06));

            // Act
            var result = applicationDbContext.LabSchedules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
