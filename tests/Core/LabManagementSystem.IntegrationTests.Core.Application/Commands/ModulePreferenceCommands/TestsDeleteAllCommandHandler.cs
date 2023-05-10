using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModulePreferenceCommands
{
    public sealed class TestsDeleteAllCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("d67e09b2-d2da-439c-886f-c93bf2a6a3dc"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("918eee18-d0f9-41ed-ab05-314861dc6021"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
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

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(3);
            foreach (var item in modulePreferences)
            {
                response.Resource.Any(x => x.UserId == item.UserId && x.ModuleId == item.ModuleId).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Command_No_Resources_To_Delete()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
