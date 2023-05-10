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
    public sealed class TestsGetDetailQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),

                new User(id: Guid.Parse("d6148930-d184-4d3f-9a60-ae5fd9e24325"), firstName: "Adam", surname: "Vega", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("cc5d36af-c7f0-4d0b-b3a4-9d76075ac892"), firstName: "Josh", surname: "Hunt", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: module.Id)
                {
                    Status = Status.Accepted,
                },
                new ModulePreference(userId: users[1].Id, moduleId: module.Id)
                {
                    Status = Status.Accepted,
                },
                new ModulePreference(userId: users[2].Id, moduleId: module.Id)
                {
                    Status = Status.PendingResponse,
                },

                new ModulePreference(userId: users[3].Id, moduleId: module.Id)
                {
                    Status = Status.Declined,
                },
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[1].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[2].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var query = new GetDetail.Query(moduleId: module.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(module.Id);
            response.Resource.Name.Should().Be(module.Name);
            response.Resource.Code.Should().Be(module.Code);

            response.Resource.ModulePreferences.Should().HaveCount(4);
            for (int i = 0; i < 4; i++)
            {
                response.Resource.ModulePreferences.Any(x => x.User.Id == modulePreferences[i].UserId && x.Module.Id == modulePreferences[i].ModuleId && x.Status == modulePreferences[i].Status.ToString()).Should().BeTrue();
            }

            response.Resource.Labs.Should().HaveCount(2);
            foreach (var item in labs)
            {
                response.Resource.Labs.Any(x => x.Id == item.Id).Should().BeTrue();
            }

            response.Resource.UserRoles.Should().HaveCount(3);
            for (int i = 0; i < 3; i++)
            {
                response.Resource.UserRoles.Any(x => x.User.Id == userModules[i].UserId && x.Role == userModules[i].Role.ToString()).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var query = new GetDetail.Query(moduleId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Has_Permission()
        {
            // Arrange
            var userId = Guid.Parse("f68fc467-f0cd-4237-a4f7-9575aa0edd1e");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            // Arrange
            var users = new List<User>()
            {
                new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),

                new User(id: Guid.Parse("d6148930-d184-4d3f-9a60-ae5fd9e24325"), firstName: "Adam", surname: "Vega", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("cc5d36af-c7f0-4d0b-b3a4-9d76075ac892"), firstName: "Josh", surname: "Hunt", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: module.Id)
                {
                    Status = Status.Accepted,
                },
                new ModulePreference(userId: users[1].Id, moduleId: module.Id)
                {
                    Status = Status.Accepted,
                },
                new ModulePreference(userId: users[2].Id, moduleId: module.Id)
                {
                    Status = Status.PendingResponse,
                },

                new ModulePreference(userId: users[3].Id, moduleId: module.Id)
                {
                    Status = Status.Declined,
                },
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[1].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[2].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var query = new GetDetail.Query(moduleId: module.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(module.Id);
            response.Resource.Name.Should().Be(module.Name);
            response.Resource.Code.Should().Be(module.Code);

            response.Resource.ModulePreferences.Should().HaveCount(4);
            for (int i = 0; i < 4; i++)
            {
                response.Resource.ModulePreferences.Any(x => x.User.Id == modulePreferences[i].UserId && x.Module.Id == modulePreferences[i].ModuleId && x.Status == modulePreferences[i].Status.ToString()).Should().BeTrue();
            }

            response.Resource.Labs.Should().HaveCount(2);
            foreach (var item in labs)
            {
                response.Resource.Labs.Any(x => x.Id == item.Id).Should().BeTrue();
            }

            response.Resource.UserRoles.Should().HaveCount(3);
            for (int i = 0; i < 3; i++)
            {
                response.Resource.UserRoles.Any(x => x.User.Id == userModules[i].UserId && x.Role == userModules[i].Role.ToString()).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            var userId = Guid.Parse("b92c6b8c-c95b-4a77-83d5-7708e2caae8c");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            // Arrange
            var users = new List<User>()
            {
                new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Beck", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("ee470b65-df89-433a-a2e6-81b8be8a3c86"), firstName: "Jack", surname: "Wood", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),

                new User(id: Guid.Parse("d6148930-d184-4d3f-9a60-ae5fd9e24325"), firstName: "Adam", surname: "Vega", achievedLevel: Level.Masters, maxWeeklyWorkHours: 40),
                new User(id: Guid.Parse("cc5d36af-c7f0-4d0b-b3a4-9d76075ac892"), firstName: "Josh", surname: "Hunt", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            await Testing.AddRangeAsync(entities: users);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: module.Id)
                {
                    Status = Status.Accepted,
                },
                new ModulePreference(userId: users[1].Id, moduleId: module.Id)
                {
                    Status = Status.Accepted,
                },
                new ModulePreference(userId: users[2].Id, moduleId: module.Id)
                {
                    Status = Status.PendingResponse,
                },

                new ModulePreference(userId: users[3].Id, moduleId: module.Id)
                {
                    Status = Status.Declined,
                },
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[1].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[2].Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var query = new GetDetail.Query(moduleId: module.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
