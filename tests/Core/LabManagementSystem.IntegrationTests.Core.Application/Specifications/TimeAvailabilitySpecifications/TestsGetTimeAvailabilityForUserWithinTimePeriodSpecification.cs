using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.TimeAvailabilitySpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.TimeAvailabilitySpecifications
{
    public sealed class TestsGetTimeAvailabilityForUserWithinTimePeriodSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("43d2e50e-9c40-41cc-8cb7-1e90d7423142"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
                new User(id: Guid.Parse("3c984102-8e73-4d19-ba2c-b2f2bb1446ca"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
            };
            await Testing.AddRangeAsync(entities: users);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(08, 00), endTime: new TimeOnly(09, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(09, 00), endTime: new TimeOnly(10, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(11, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(11, 00), endTime: new TimeOnly(12, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),

                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(16, 00), endTime: new TimeOnly(17, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(17, 00), endTime: new TimeOnly(18, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetTimeAvailabilityForUserWithinTimePeriodSpecification(userId: users[0].Id,
                                                                                            day: WorkDayOfWeek.Monday,
                                                                                            startTime: new TimeOnly(10, 00),
                                                                                            endTime: new TimeOnly(12, 00));

            // Act
            var result = applicationDbContext.TimeAvailabilities.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(2);

            result.Any(x => x.Day != WorkDayOfWeek.Monday).Should().BeFalse();
            result.Any(x => x.StartTime == new TimeOnly(10, 00)).Should().BeTrue();
            result.Any(x => x.EndTime == new TimeOnly(11, 00)).Should().BeTrue();

            result.Any(x => x.StartTime == new TimeOnly(11, 00)).Should().BeTrue();
            result.Any(x => x.EndTime == new TimeOnly(12, 00)).Should().BeTrue();
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetTimeAvailabilityForUserWithinTimePeriodSpecification(userId: Guid.Parse("43d2e50e-9c40-41cc-8cb7-1e90d7423142"),
                                                                                            day: WorkDayOfWeek.Tuesday,
                                                                                            startTime: new TimeOnly(10, 00),
                                                                                            endTime: new TimeOnly(12, 00));

            // Act
            var result = applicationDbContext.TimeAvailabilities.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
