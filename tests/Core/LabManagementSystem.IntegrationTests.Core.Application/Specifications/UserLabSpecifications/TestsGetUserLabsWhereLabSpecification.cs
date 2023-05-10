using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.UserLabSpecifications
{
    public sealed class TestsGetUserLabsWhereLabSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("d64ee5d3-5fd4-4dcf-b183-23fe295d4d9e"), firstName: "A", surname: "A", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("7a44f505-5b06-4a50-bd6a-be088debdd70"), firstName: "B", surname: "B", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("d9f69299-708e-4bcc-882b-bc451c02013d"), firstName: "C", surname: "C", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
                new User(id: Guid.Parse("75d58ed6-a0c3-47fa-ba2f-abfaf0ce767d"), firstName: "D", surname: "D", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("481c69ae-7253-4409-a404-f551e7bfe3b8"), firstName: "E", surname: "E", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 4),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 4),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[2].Id, labId: labs[0].Id),
                new UserLab(userId: users[3].Id, labId: labs[1].Id),
                new UserLab(userId: users[4].Id, labId: labs[1].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserLabsWhereLabSpecification(labId: labs[0].Id);

            // Act
            var result = applicationDbContext.UserLabs.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(3);
            result.Any(x => x.LabId == labs[1].Id).Should().BeFalse();
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserLabsWhereLabSpecification(labId: Guid.Parse("5068888a-4fae-4177-b3ed-9c4afec7df12"));

            // Act
            var result = applicationDbContext.UserLabs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
