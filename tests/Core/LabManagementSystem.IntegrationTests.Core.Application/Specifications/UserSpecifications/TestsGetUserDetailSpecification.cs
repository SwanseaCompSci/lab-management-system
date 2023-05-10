using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.UserSpecifications
{
    public sealed class TestsGetUserDetailSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("4bfc577b-6981-4c4f-b7ae-d6652b892fb9"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
                new User(id: Guid.Parse("cf02479d-0b5e-4510-8273-fbf146fcc5f7"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20),
            };
            await Testing.AddRangeAsync(entities: users);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 1", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var modulePreferences = new List<ModulePreference>()
            {
                new ModulePreference(userId: users[0].Id, moduleId: modules[0].Id)
                {
                    Status = Status.Accepted,
                },
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.TeachingAssistant),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(11, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(11, 00), endTime: new TimeOnly(12, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserDetailSpecification(userId: users[0].Id);

            // Act
            var result = applicationDbContext.Users.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(users[0].Id);
            result[0].ModulePreferences.Should().HaveCount(1);
            result[0].UserModules.Should().HaveCount(1);
            result[0].TimeAvailabilities.Should().HaveCount(4);
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserDetailSpecification(userId: Guid.Parse("0e6d1d68-8cb3-4f2c-87ba-6dd5096eb0ba"));

            // Act
            var result = applicationDbContext.Users.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
