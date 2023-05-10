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
    public sealed class TestsGetAllModulesWherePermissionSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
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
                new UserModule(userId: user.Id, moduleId: modules[0].Id, role: ModuleRole.LabCoordinator),
            };
            await Testing.AddRangeAsync(entities: userModules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetAllModulesWherePermissionSpecification(userId: user.Id);

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(modules[0].Id);
        }
    }
}
