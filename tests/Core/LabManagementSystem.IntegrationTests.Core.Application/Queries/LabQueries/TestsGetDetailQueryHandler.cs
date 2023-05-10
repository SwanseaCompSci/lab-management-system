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

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(16, 00), endTime: new TimeOnly(17, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 3, maxNumberOfStaff: 4),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[2].Id, labId: labs[0].Id),

                new UserLab(userId: users[3].Id, labId: labs[1].Id),
                new UserLab(userId: users[4].Id, labId: labs[1].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 09, 16, 00, 00), end: new DateTime(2023, 02, 09, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 16, 16, 00, 00), end: new DateTime(2023, 02, 16, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 23, 16, 00, 00), end: new DateTime(2023, 02, 23, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 03, 02, 16, 00, 00), end: new DateTime(2023, 03, 02, 17, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var query = new GetDetail.Query(labId: labs[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(labs[0].Id);
            response.Resource.Name.Should().Be(labs[0].Name);
            response.Resource.Day.Should().Be(labs[0].Day);
            response.Resource.StartTime.Should().Be(labs[0].StartTime);
            response.Resource.EndTime.Should().Be(labs[0].EndTime);
            response.Resource.MinNumberOfStaff.Should().Be(labs[0].MinNumberOfStaff);
            response.Resource.MaxNumberOfStaff.Should().Be(labs[0].MaxNumberOfStaff);

            response.Resource.Users.Should().HaveCount(3);
            for (var i = 0; i < 3; i++)
            {
                response.Resource.Users.Any(x => x.Id == users[i].Id).Should().BeTrue();
            }

            response.Resource.LabSchedules.Should().HaveCount(4);
            for (var i = 0; i < 4; i++)
            {
                response.Resource.LabSchedules.Any(x => x.Id == labSchedules[i].Id).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var query = new GetDetail.Query(labId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Has_Permission()
        {
            // Arrange
            var userId = Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

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

            var userModule = new UserModule(userId: userId, moduleId: module.Id, role: ModuleRole.TeachingAssistant);
            await Testing.AddAsync(entity: userModule);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(16, 00), endTime: new TimeOnly(17, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 3, maxNumberOfStaff: 4),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[2].Id, labId: labs[0].Id),

                new UserLab(userId: users[3].Id, labId: labs[1].Id),
                new UserLab(userId: users[4].Id, labId: labs[1].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 06, 16, 00, 00), end: new DateTime(2023, 02, 06, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 13, 16, 00, 00), end: new DateTime(2023, 02, 13, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 20, 16, 00, 00), end: new DateTime(2023, 02, 20, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 27, 16, 00, 00), end: new DateTime(2023, 02, 27, 17, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var query = new GetDetail.Query(labId: labs[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(labs[0].Id);
            response.Resource.Name.Should().Be(labs[0].Name);
            response.Resource.Day.Should().Be(labs[0].Day);
            response.Resource.StartTime.Should().Be(labs[0].StartTime);
            response.Resource.EndTime.Should().Be(labs[0].EndTime);
            response.Resource.MinNumberOfStaff.Should().Be(labs[0].MinNumberOfStaff);
            response.Resource.MaxNumberOfStaff.Should().Be(labs[0].MaxNumberOfStaff);

            response.Resource.Users.Should().HaveCount(3);
            for (var i = 0; i < 3; i++)
            {
                response.Resource.Users.Any(x => x.Id == users[i].Id).Should().BeTrue();
            }

            response.Resource.LabSchedules.Should().HaveCount(4);
            for (var i = 0; i < 4; i++)
            {
                response.Resource.LabSchedules.Any(x => x.Id == labSchedules[i].Id).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            var userId = Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

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

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 3, maxNumberOfStaff: 4),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[2].Id, labId: labs[0].Id),

                new UserLab(userId: users[3].Id, labId: labs[1].Id),
                new UserLab(userId: users[4].Id, labId: labs[1].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 06, 16, 00, 00), end: new DateTime(2023, 02, 06, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 13, 16, 00, 00), end: new DateTime(2023, 02, 13, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 20, 16, 00, 00), end: new DateTime(2023, 02, 20, 17, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 27, 16, 00, 00), end: new DateTime(2023, 02, 27, 17, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var query = new GetDetail.Query(labId: labs[0].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
