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
    public sealed class TestsGetDetailQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("740b7ba5-2760-4622-a173-7ab067f6544c"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("5417b552-5210-488b-b955-274b91f15f0b"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
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
                new ModulePreference(userId: users[1].Id, moduleId: modules[1].Id),
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[1].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),

                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(08, 00), endTime: new TimeOnly(09, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(09, 00), endTime: new TimeOnly(10, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(11, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(11, 00), endTime: new TimeOnly(12, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var query = new GetDetail.Query(userId: users[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(users[0].Id);
            response.Resource.FirstName.Should().Be(users[0].FirstName);
            response.Resource.Surname.Should().Be(users[0].Surname);
            response.Resource.AchievedLevel.Should().Be(users[0].AchievedLevel.ToString());
            response.Resource.MaxWeeklyWorkHours.Should().Be(users[0].MaxWeeklyWorkHours);

            response.Resource.ModulePreferences.Should().HaveCount(1);
            response.Resource.ModulePreferences.First().Module.Id.Should().Be(modules[0].Id);

            response.Resource.ModuleRoles.Should().HaveCount(1);
            response.Resource.ModuleRoles.First().Module.Id.Should().Be(modules[0].Id);
            response.Resource.ModuleRoles.First().Role.Should().Be(userModules[0].Role.ToString());

            response.Resource.TimeAvailabilities.Should().HaveCount(4);
            for (int i = 0; i < 4; i++)
            {
                response.Resource.TimeAvailabilities
                    .Any(x => x.UserId == timeAvailabilities[i].UserId && x.Day == timeAvailabilities[i].Day && x.StartTime == timeAvailabilities[i].StartTime && x.EndTime == timeAvailabilities[i].EndTime)
                    .Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetDetail.Query(userId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
