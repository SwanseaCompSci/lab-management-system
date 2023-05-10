using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.ModulePreferenceSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.ModulePreferenceSpecifications
{
    public sealed class TestsGetAllModulePreferenceDetailsSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("e0bcea2b-bea7-4e85-985a-cfa6c774885b"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("de213b60-e7cd-449a-89dc-6420906983dd"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
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
                new ModulePreference(userId: users[0].Id, moduleId: modules[1].Id),

                new ModulePreference(userId: users[1].Id, moduleId: modules[1].Id),
            };
            await Testing.AddRangeAsync(entities: modulePreferences);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetAllModulePreferenceDetailsSpecification();

            // Act
            var response = applicationDbContext.ModulePreferences.WithSpecification(specification).ToList();

            // Assert
            response.Should().HaveCount(3);
            foreach (var item in modulePreferences)
            {
                response.Any(x => x.User.Id == item.UserId && x.Module.Id == item.ModuleId).Should().BeTrue();
            }
        }
    }
}
