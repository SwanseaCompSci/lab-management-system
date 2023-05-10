using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.ModuleQueries
{
    public sealed class TestsGetAllQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_As_Administrator_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),

                new Module(name: "Concepts of Computer Science 1", code: "CS-150", level: Level.Year1),
                new Module(name: "Concepts of Computer Science 2", code: "CS-155", level: Level.Year1),

                new Module(name: "Modelling Computing Systems 1", code: "CS-170", level: Level.Year1),
                new Module(name: "Modelling Computing Systems 2", code: "CS-175", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var query = new GetAll.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(6);
        }

        [Test]
        public async Task Handle_Query_As_Administrator_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var query = new GetAll.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_Query_As_User_Entities_Found_Has_Permission()
        {
            // Arrange
            var userId = Guid.Parse("e58118b3-c7aa-4097-9e24-091b41cb1535");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            var user = new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),

                new Module(name: "Concepts of Computer Science 1", code: "CS-150", level: Level.Year1),
                new Module(name: "Concepts of Computer Science 2", code: "CS-155", level: Level.Year1),

                new Module(name: "Modelling Computing Systems 1", code: "CS-170", level: Level.Year1),
                new Module(name: "Modelling Computing Systems 2", code: "CS-175", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: userId, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: userId, moduleId: modules[2].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: userId, moduleId: modules[4].Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var query = new GetAll.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(3);
            for (int i = 0; i < 3; i++)
            {
                response.Resource.Any(x => x.Id == modules[i * 2].Id).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_As_User_Entities_Found_Does_Not_Have_Permission()
        {
            // Arrange
            var userId = Guid.Parse("cfce4c31-1fae-4b14-bbd5-67c44872f357");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            var user = new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),

                new Module(name: "Concepts of Computer Science 1", code: "CS-150", level: Level.Year1),
                new Module(name: "Concepts of Computer Science 2", code: "CS-155", level: Level.Year1),

                new Module(name: "Modelling Computing Systems 1", code: "CS-170", level: Level.Year1),
                new Module(name: "Modelling Computing Systems 2", code: "CS-175", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var query = new GetAll.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
