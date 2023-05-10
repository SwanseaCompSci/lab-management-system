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
    public sealed class TestsGetModuleWherePermissionSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found_Has_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("cb419b93-43f1-4271-bc9f-02858e74c6c3"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: user.Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleWherePermissionSpecification(moduleId: modules[0].Id, userId: user.Id);

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(modules[0].Id);
        }

        [Test]
        public async Task Specified_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("cb419b93-43f1-4271-bc9f-02858e74c6c3"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var userModules = new List<UserModule>()
            {
                new UserModule(userId: user.Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleWherePermissionSpecification(moduleId: modules[1].Id, userId: user.Id);

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

            var specification = new GetModuleWherePermissionSpecification(moduleId: Guid.Parse("1d6f3bee-56b9-4772-a410-15cea22d17b3"),
                                                                          userId: Guid.Parse("4482d5b8-e487-488d-92a1-ee6cf4c0ccc4"));

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
