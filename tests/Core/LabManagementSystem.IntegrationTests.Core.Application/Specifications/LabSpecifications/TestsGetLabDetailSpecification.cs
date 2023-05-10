using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.LabSpecifications
{
    public sealed class TestsGetLabDetailSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(15, 00, 00), endTime: new TimeOnly(16, 00, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00, 00), endTime: new TimeOnly(13, 00, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var user = new User(id: Guid.Parse("7490ee34-1ebc-48f8-82c0-dfcfd7cea954"),
                                firstName: "Jessica",
                                surname: "Pearson",
                                achievedLevel: Level.Year2,
                                maxWeeklyWorkHours: 40);
            await Testing.AddAsync(entity: user);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: user.Id, labId: labs[0].Id),
                new UserLab(userId: user.Id, labId: labs[1].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 08, 15, 00, 00), end: new DateTime(2023, 02, 08, 16, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 15, 15, 00, 00), end: new DateTime(2023, 02, 15, 16, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 22, 15, 00, 00), end: new DateTime(2023, 02, 22, 16, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 03, 01, 15, 00, 00), end: new DateTime(2023, 03, 01, 16, 00, 00)),

                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 09, 12, 00, 00), end: new DateTime(2023, 02, 09, 13, 00, 00)),
                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 16, 12, 00, 00), end: new DateTime(2023, 02, 16, 13, 00, 00)),
                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 23, 12, 00, 00), end: new DateTime(2023, 02, 23, 13, 00, 00)),
                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 02, 12, 00, 00), end: new DateTime(2023, 03, 02, 13, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabDetailSpecification(labId: labs[0].Id);

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Specification
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(labs[0].Id);
            result[0].UserLabs.Should().HaveCount(1);

            var resultUserLab = result[0].UserLabs.FirstOrDefault(x => x.LabId == labs[0].Id) ?? throw new NullReferenceException();
            resultUserLab.User.Should().NotBeNull();

            result[0].LabSchedules.Should().HaveCount(4);
            result[0].LabSchedules.Any(x => x.LabId != labs[0].Id).Should().BeFalse();
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabDetailSpecification(labId: Guid.Parse("6e929822-4961-4c5f-99c0-c48d2866bae6"));

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
