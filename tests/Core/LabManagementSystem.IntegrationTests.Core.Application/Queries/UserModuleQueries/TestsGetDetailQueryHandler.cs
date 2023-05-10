using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.UserModuleQueries
{
    public sealed class TestsGetDetailQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
            };
            await Testing.AddRangeAsync(entities: users);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[1].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[2].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var query = new GetDetail.Query(userId: userModules[1].UserId, moduleId: userModules[1].ModuleId);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.UserId.Should().Be(userModules[1].UserId);
            response.Resource.ModuleId.Should().Be(userModules[1].ModuleId);
            response.Resource.Role.Should().Be(userModules[1].Role.ToString());

            response.Resource.UserFirstName.Should().Be(users[1].FirstName);
            response.Resource.UserSurname.Should().Be(users[1].Surname);
            response.Resource.ModuleName.Should().Be(modules[0].Name);
        }

        [Test]
        public async Task Handle_Query_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetDetail.Query(userId: Guid.NewGuid(), moduleId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
