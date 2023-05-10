using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.ModuleQueries
{
    public sealed class TestsGetQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var query = new Get.Query(moduleId: modules[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(modules[0].Id);
            response.Resource!.Name.Should().Be(modules[0].Name);
            response.Resource!.Code.Should().Be(modules[0].Code);
            response.Resource!.Level.Should().Be(modules[0].Level.ToString());
        }

        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());
            var query = new Get.Query(moduleId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Has_Permission()
        {
            // Arrange
            var userId = Guid.Parse("0ee33381-690a-41da-be4c-1feedee00242");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            var user = new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var userModule = new UserModule(userId: userId, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant);
            await Testing.AddAsync(entity: userModule);

            var query = new Get.Query(moduleId: modules[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(modules[0].Id);
            response.Resource!.Name.Should().Be(modules[0].Name);
            response.Resource!.Code.Should().Be(modules[0].Code);
            response.Resource!.Level.Should().Be(modules[0].Level.ToString());
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            var userId = Guid.Parse("0ee33381-690a-41da-be4c-1feedee00242");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            var user = new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var query = new Get.Query(moduleId: modules[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
