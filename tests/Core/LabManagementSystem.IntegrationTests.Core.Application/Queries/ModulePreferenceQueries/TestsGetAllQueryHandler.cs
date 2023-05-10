using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModulePreferenceQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.ModulePreferenceQueries
{
    public sealed class TestsGetAllQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("dd8893ba-1d86-4692-8de7-7be2e3dd8419"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("3b0b1e4a-bc33-440f-8233-2c6a8216582a"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
            };
            await Testing.AddRangeAsync(entities: users);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: modules[0].Id),
                new ModulePreference(userId: users[0].Id, moduleId: modules[1].Id),

                new ModulePreference(userId: users[1].Id, moduleId: modules[1].Id),
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var query = new GetAll.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(3);
            foreach (var item in modulePreferences)
            {
                response.Resource.Any(x => x.User.Id == item.UserId && x.Module.Id == item.ModuleId).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetAll.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
