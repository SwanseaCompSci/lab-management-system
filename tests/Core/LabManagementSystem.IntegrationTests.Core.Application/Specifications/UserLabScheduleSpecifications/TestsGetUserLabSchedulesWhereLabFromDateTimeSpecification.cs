using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabScheduleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.UserLabScheduleSpecifications
{
    public sealed class TestsGetUserLabSchedulesWhereLabFromDateTimeSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("0cceab51-8973-47f5-9ff7-4a9dfc3688af"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3);
            await Testing.AddAsync(entity: lab);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 07, 13, 00, 00), end: new DateTime(2023, 02, 07, 14, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 14, 13, 00, 00), end: new DateTime(2023, 02, 14, 14, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 21, 13, 00, 00), end: new DateTime(2023, 02, 21, 14, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 28, 13, 00, 00), end: new DateTime(2023, 02, 28, 14, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var userLabSchedules = new List<UserLabSchedule>()
            {
                new UserLabSchedule(userId: user.Id, labScheduleId: labSchedules[0].Id),
                new UserLabSchedule(userId: user.Id, labScheduleId: labSchedules[1].Id),
                new UserLabSchedule(userId: user.Id, labScheduleId: labSchedules[2].Id),
                new UserLabSchedule(userId: user.Id, labScheduleId: labSchedules[3].Id),
            };
            await Testing.AddRangeAsync(entities: userLabSchedules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var dateTime = new DateTime(2023, 02, 15);
            var specification = new GetUserLabSchedulesWhereLabFromDateTimeSpecification(userId: user.Id,
                                                                                         labId: lab.Id,
                                                                                         dateTime: dateTime);

            // Act
            var result = applicationDbContext.UserLabSchedules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Any(x => x.LabSchedule.Start < dateTime).Should().BeFalse();
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserLabSchedulesWhereLabFromDateTimeSpecification(userId: Guid.Parse("6e2213ca-9939-4aad-9ab8-5eb6faee0898"),
                                                                                         labId: Guid.Parse("4cc6f79c-57cd-47db-a691-06c9974022f4"),
                                                                                         dateTime: new DateTime(2023, 02, 15, 14, 00, 00));

            // Act
            var result = applicationDbContext.UserLabSchedules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
