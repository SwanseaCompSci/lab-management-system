using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserModuleSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.UserModuleSpecifications
{
    public sealed class TestsGetUserModuleDetailSpecification : TestBase
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
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
                new UserModule(userId: users[1].Id, moduleId: modules[1].Id, role: ModuleRole.LabCoordinator),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserModuleDetailSpecification(userId: users[0].Id, moduleId: modules[0].Id);

            // Act
            var result = applicationDbContext.UserModules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].UserId.Should().Be(users[0].Id);
            result[0].ModuleId.Should().Be(modules[0].Id);
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserModuleDetailSpecification(userId: Guid.Parse("f476515c-eae0-449b-9f14-7ac8fd22837c"),
                                                                     moduleId: Guid.Parse("38470d5d-2463-4951-a434-007c92fb6e0d"));

            // Act
            var result = applicationDbContext.UserModules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
