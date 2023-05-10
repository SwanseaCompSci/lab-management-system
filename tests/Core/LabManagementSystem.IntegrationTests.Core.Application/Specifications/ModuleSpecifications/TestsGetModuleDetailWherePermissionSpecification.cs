using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModuleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.ModuleSpecifications
{
    public sealed class TestsGetModuleDetailWherePermissionSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found_Has_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("94ab8767-bef9-49db-9aa9-bdbf68fbd411"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("b5364b66-264c-4b0e-8508-211748ff8992"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
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
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: modules[1].Id, name: "Turring", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
                new UserModule(userId: users[1].Id, moduleId: modules[1].Id, role: ModuleRole.ModuleCoordinator),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: modules[0].Id),
                new ModulePreference(userId: users[1].Id, moduleId: modules[1].Id),
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleDetailWherePermissionSpecification(moduleId: modules[0].Id, userId: users[0].Id);

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(modules[0].Id);
            result[0].Labs.Should().HaveCount(1);
            result[0].UserModules.Should().HaveCount(1);
            result[0].UserModules.First().User.Id.Should().Be(users[0].Id);
            result[0].ModulePreferences.Should().HaveCount(1);
            result[0].ModulePreferences.First().User.Id.Should().Be(users[0].Id);
        }

        [Test]
        public async Task Specified_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("94ab8767-bef9-49db-9aa9-bdbf68fbd411"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
                new User(id: Guid.Parse("b5364b66-264c-4b0e-8508-211748ff8992"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
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
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: modules[1].Id, name: "Turring", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
                new UserModule(userId: users[1].Id, moduleId: modules[1].Id, role: ModuleRole.ModuleCoordinator),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: modules[0].Id),
                new ModulePreference(userId: users[1].Id, moduleId: modules[1].Id),
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleDetailWherePermissionSpecification(moduleId: modules[0].Id, userId: users[1].Id);

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleDetailWherePermissionSpecification(moduleId: Guid.Parse("23329ac3-b450-42d6-89e7-a75c8de794e8"),
                                                                                userId: Guid.Parse("3b0f918b-0afa-48f4-83aa-97b6ae4090f8"));

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
