using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.UserQueries
{
    public sealed class TestsSearchInModuleButNotInLabQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),

                new Lab(moduleId: modules[1].Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[1].Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),

                new User(id: Guid.Parse("d6148930-d184-4d3f-9a60-ae5fd9e24325"), firstName: "Adam", surname: "Vega", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("cc5d36af-c7f0-4d0b-b3a4-9d76075ac892"), firstName: "Josh", surname: "Hunt", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[1].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[2].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),

                new UserModule(userId: users[3].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[4].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var userLabs = new List<UserLab>()
            {
                // Programming 1
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[2].Id, labId: labs[0].Id),

                new UserLab(userId: users[0].Id, labId: labs[1].Id),

                // Programming 2
                new UserLab(userId: users[3].Id, labId: labs[2].Id),
                new UserLab(userId: users[4].Id, labId: labs[3].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var query = new SearchInModuleButNotInLab.Query(moduleId: modules[0].Id, labId: labs[1].Id, searchExpression: "k");

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(2);
            response.Resource.Any(x => x.Id == users[1].Id).Should().BeTrue();
            response.Resource.Any(x => x.Id == users[2].Id).Should().BeTrue();
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new SearchInModuleButNotInLab.Query(moduleId: Guid.NewGuid(), labId: Guid.NewGuid(), searchExpression: "e");

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
