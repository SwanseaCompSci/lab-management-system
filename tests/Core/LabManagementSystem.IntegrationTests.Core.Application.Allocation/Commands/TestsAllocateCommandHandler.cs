using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Commands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Allocation.Commands
{
    public sealed class TestsAllocateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("81c713d5-92c3-49cb-91c1-ce7e2219ae21"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("a78936d3-f38b-46e9-9139-8cd6fe7cd718"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("a640d6df-d09f-402b-9864-7e65c15e81a0"), firstName: "Pete", surname: "Beck", achievedLevel: Level.Year3, maxWeeklyWorkHours: 10),

                new User(id: Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"), firstName: "Jack", surname: "Bush", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("82290a26-4103-4929-8ba0-25c59950b6f8"), firstName: "Alex", surname: "Falk", achievedLevel: Level.Year3, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("6775721b-db5d-4a2b-96ca-93912190d93c"), firstName: "Elon", surname: "Musk", achievedLevel: Level.Year3, maxWeeklyWorkHours: 10),

                new User(id: Guid.Parse("1183addd-91c0-4443-8147-3c63090e4fbe"), firstName: "Emma", surname: "Ford", achievedLevel: Level.Masters, maxWeeklyWorkHours: 10),
            };
            await Testing.AddRangeAsync(entities: users);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),

                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),

                new TimeAvailability(userId: users[2].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[2].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[2].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[2].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),

                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(16, 00), endTime: new TimeOnly(17, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(17, 00), endTime: new TimeOnly(18, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(11, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(11, 00), endTime: new TimeOnly(12, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[3].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),

                new TimeAvailability(userId: users[4].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(11, 00)),
                new TimeAvailability(userId: users[4].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(11, 00), endTime: new TimeOnly(12, 00)),
                new TimeAvailability(userId: users[4].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[4].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),

                new TimeAvailability(userId: users[5].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(11, 00)),
                new TimeAvailability(userId: users[5].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(11, 00), endTime: new TimeOnly(12, 00)),
                new TimeAvailability(userId: users[5].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[5].Id, day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Concurrency", code: "CS-210", level: Level.Year2),
            };
            await Testing.AddRangeAsync(entities: modules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Group 1", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 3, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Group 2", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(16, 00), minNumberOfStaff: 3, maxNumberOfStaff: 5),
                
                new Lab(moduleId: modules[1].Id, name: "Group 1", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(12, 00), minNumberOfStaff: 3, maxNumberOfStaff: 4),
                new Lab(moduleId: modules[1].Id, name: "Group 2", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 3, maxNumberOfStaff: 4),

            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[0].Id, labId: labs[1].Id),

                new UserLab(userId: users[6].Id, labId: labs[2].Id),
                new UserLab(userId: users[6].Id, labId: labs[3].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: modules[0].Id)
                {
                    Status = Status.Accepted,
                },

                new ModulePreference(userId: users[3].Id, moduleId: modules[0].Id)
                {
                    Status = Status.Declined,
                },
                new ModulePreference(userId: users[3].Id, moduleId: modules[1].Id)
                {
                    Status = Status.Accepted,
                },

                new ModulePreference(userId: users[4].Id, moduleId: modules[1].Id)
                {
                    Status = Status.Accepted,
                },
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var command = new Allocate.Command(algorithm: "FirstMatch");

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(14);

            response.Resource.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[1].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[1].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();

            response.Resource.Any(x => x.UserId == users[3].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[3].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[4].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[4].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[5].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[5].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();

            response.Resource.Any(x => x.UserId == users[6].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            response.Resource.Any(x => x.UserId == users[6].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
        }
    }
}
