using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.DashboardQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.DashboardQueries
{
    public sealed class TestsGetFutureWorkShiftsQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());
            Testing.RunOnDateTime(dateTime: new DateTime(2023, 02, 09));

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Group 1", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Group 2", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(16, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 06, 12, 00, 00), end: new DateTime(2023, 02, 06, 14, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 13, 12, 00, 00), end: new DateTime(2023, 02, 13, 14, 00, 00)),

                new LabSchedule(labId: labs[1].Id, start: new DateTime(2023, 02, 10, 14, 00, 00), end: new DateTime(2023, 02, 10, 16, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var users = new List<User>()
            {
                new User(id: Guid.Parse("8edca4a0-8802-4a0b-a77f-96657cea2a62"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("5f94cbfc-a5b6-4d03-ab4a-b0ed9bbb8eff"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
            };
            await Testing.AddRangeAsync(entities: users);

            var userLabSchedules = new List<UserLabSchedule>()
            {
                new UserLabSchedule(userId: users[0].Id, labScheduleId: labSchedules[0].Id),
                new UserLabSchedule(userId: users[0].Id, labScheduleId: labSchedules[1].Id),

                new UserLabSchedule(userId: users[1].Id, labScheduleId: labSchedules[2].Id),
            };
            await Testing.AddRangeAsync(entities: userLabSchedules);

            var query = new GetFutureWorkShifts.Query(userId: users[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(1);

            response.Resource.First().LabScheduleId.Should().Be(labSchedules[1].Id);
            response.Resource.First().LabScheduleStart.Should().Be(labSchedules[1].Start);
            response.Resource.First().LabScheduleEnd.Should().Be(labSchedules[1].End);

            response.Resource.First().LabId.Should().Be(labs[0].Id);
            response.Resource.First().LabName.Should().Be(labs[0].Name);

            response.Resource.First().ModuleId.Should().Be(modules[0].Id);
            response.Resource.First().ModuleName.Should().Be(modules[0].Name);
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());
            Testing.RunOnDateTime(dateTime: new DateTime(2023, 02, 09));

            var query = new GetFutureWorkShifts.Query(userId: Guid.Parse("ca41ba1b-e708-4fc7-8808-4015cf34fc51"));

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeEmpty();
        }
    }
}
