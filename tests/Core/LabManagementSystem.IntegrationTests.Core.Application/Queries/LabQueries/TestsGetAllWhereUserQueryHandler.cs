using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.LabQueries
{
    public sealed class TestsGetAllWhereUserQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("02596755-cd3b-4b7d-8497-3128b36f86d0"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("8648737b-4ccc-442d-a25d-d845695f3349"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
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
                new Lab(moduleId: modules[0].Id, name: "Group 1", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 3, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Group 2", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(16, 00), minNumberOfStaff: 3, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Group 3", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(16, 00), endTime: new TimeOnly(18, 00), minNumberOfStaff: 3, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[0].Id, labId: labs[1].Id),

                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[1].Id),
                new UserLab(userId: users[1].Id, labId: labs[2].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var query = new GetAllWhereUser.Query(userId: users[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(2);

            for (int i = 0; i < 2; i++)
            {
                response.Resource.Any(x => x.Id == labs[i].Id).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetAllWhereUser.Query(userId: Guid.Parse("d330fa64-04b3-41d4-8844-ddfa94339408"));

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
