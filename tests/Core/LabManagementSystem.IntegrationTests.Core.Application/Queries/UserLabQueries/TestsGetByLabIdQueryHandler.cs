using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserLabQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.UserLabQueries
{
    public sealed class TestsGetByLabIdQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
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

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: modules[1].Id, name: "Turring", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
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

            var query = new GetByLabId.Query(labId: labs[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(3);
            for (var i = 0; i < 3; i++)
            {
                response.Resource.Any(x => x.UserId == userLabs[i].UserId && x.LabId == userLabs[i].LabId).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetByLabId.Query(labId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
