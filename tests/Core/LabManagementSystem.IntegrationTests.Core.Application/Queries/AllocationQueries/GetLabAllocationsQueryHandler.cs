using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.AllocationQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.AllocationQueries
{
    public sealed class GetLabAllocationsQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

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

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[1].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[2].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),

                new UserModule(userId: users[3].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[4].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[5].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
                new UserModule(userId: users[6].Id, moduleId: modules[1].Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
                new UserLab(userId: users[0].Id, labId: labs[1].Id),
                new UserLab(userId: users[1].Id, labId: labs[0].Id),
                new UserLab(userId: users[1].Id, labId: labs[1].Id),
                new UserLab(userId: users[2].Id, labId: labs[0].Id),
                new UserLab(userId: users[2].Id, labId: labs[1].Id),

                new UserLab(userId: users[3].Id, labId: labs[2].Id),
                new UserLab(userId: users[3].Id, labId: labs[3].Id),
                new UserLab(userId: users[4].Id, labId: labs[2].Id),
                new UserLab(userId: users[4].Id, labId: labs[3].Id),
                new UserLab(userId: users[5].Id, labId: labs[2].Id),
                new UserLab(userId: users[5].Id, labId: labs[3].Id),

                new UserLab(userId: users[6].Id, labId: labs[2].Id),
                new UserLab(userId: users[6].Id, labId: labs[3].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var query = new GetLabAllocations.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(4);

            foreach (var lab in labs)
            {
                var allocation = response.Resource.FirstOrDefault(x => x.Lab.Id == lab.Id) ?? throw new NullReferenceException();

                allocation.Lab.Id.Should().Be(lab.Id);
                allocation.Lab.ModuleId.Should().Be(lab.ModuleId);
                allocation.Lab.Name.Should().Be(lab.Name);
                allocation.Lab.Day.Should().Be(lab.Day);
                allocation.Lab.StartTime.Should().Be(lab.StartTime);
                allocation.Lab.EndTime.Should().Be(lab.EndTime);
                allocation.Lab.MinNumberOfStaff.Should().Be(lab.MinNumberOfStaff);
                allocation.Lab.MaxNumberOfStaff.Should().Be(lab.MaxNumberOfStaff);

                var module = modules.FirstOrDefault(x => x.Id == lab.ModuleId) ?? throw new NullReferenceException();
                allocation.Module.Id.Should().Be(module.Id);
                allocation.Module.Name.Should().Be(module.Name);
                allocation.Module.Code.Should().Be(module.Code);
                allocation.Module.Level.Should().Be(module.Level.ToString());

                foreach (var userLab in userLabs.Where(x => x.LabId == lab.Id))
                {
                    var user = users.FirstOrDefault(x => x.Id == userLab.UserId) ?? throw new NullReferenceException();
                    var allocatedUser = allocation.AllocatedUsers.FirstOrDefault(x => x.Id == userLab.UserId) ?? throw new NullReferenceException();

                    allocatedUser.Id.Should().Be(user.Id);
                    allocatedUser.FirstName.Should().Be(user.FirstName);
                    allocatedUser.Surname.Should().Be(user.Surname);
                    allocatedUser.AchievedLevel.Should().Be(user.AchievedLevel.ToString());
                    allocatedUser.MaxWeeklyWorkHours.Should().Be(user.MaxWeeklyWorkHours);
                    allocatedUser.QuestionnaireToken.Should().Be(user.QuestionnaireToken);
                }
            }
        }

        [Test]
        public async Task Handle_Query_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new GetLabAllocations.Query();

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().HaveCount(0);
        }
    }
}
