using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.AllocationSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.AllocationSpecifications
{
    public sealed class TestsGetAllAllocationResultsSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
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

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetAllAllocationResultsSpecification();

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(4);

            var responseLab1 = result.FirstOrDefault(x => x.Id == labs[0].Id) ?? throw new NullReferenceException();
            responseLab1.Module.Id.Should().Be(modules[0].Id);
            responseLab1.UserLabs.Any(x => x.User.Id == users[0].Id).Should().BeTrue();
            responseLab1.UserLabs.Any(x => x.User.Id == users[1].Id).Should().BeTrue();
            responseLab1.UserLabs.Any(x => x.User.Id == users[2].Id).Should().BeTrue();

            var responseLab2 = result.FirstOrDefault(x => x.Id == labs[1].Id) ?? throw new NullReferenceException();
            responseLab2.Module.Id.Should().Be(modules[0].Id);
            responseLab2.UserLabs.Any(x => x.User.Id == users[0].Id).Should().BeTrue();
            responseLab2.UserLabs.Any(x => x.User.Id == users[1].Id).Should().BeTrue();
            responseLab2.UserLabs.Any(x => x.User.Id == users[2].Id).Should().BeTrue();

            var responseLab3 = result.FirstOrDefault(x => x.Id == labs[2].Id) ?? throw new NullReferenceException();
            responseLab3.Module.Id.Should().Be(modules[1].Id);
            responseLab3.UserLabs.Any(x => x.User.Id == users[3].Id).Should().BeTrue();
            responseLab3.UserLabs.Any(x => x.User.Id == users[4].Id).Should().BeTrue();
            responseLab3.UserLabs.Any(x => x.User.Id == users[5].Id).Should().BeTrue();
            responseLab3.UserLabs.Any(x => x.User.Id == users[6].Id).Should().BeTrue();

            var responseLab4 = result.FirstOrDefault(x => x.Id == labs[3].Id) ?? throw new NullReferenceException();
            responseLab4.Module.Id.Should().Be(modules[1].Id);
            responseLab4.UserLabs.Any(x => x.User.Id == users[3].Id).Should().BeTrue();
            responseLab4.UserLabs.Any(x => x.User.Id == users[4].Id).Should().BeTrue();
            responseLab4.UserLabs.Any(x => x.User.Id == users[5].Id).Should().BeTrue();
            responseLab4.UserLabs.Any(x => x.User.Id == users[6].Id).Should().BeTrue();
        }
    }
}
