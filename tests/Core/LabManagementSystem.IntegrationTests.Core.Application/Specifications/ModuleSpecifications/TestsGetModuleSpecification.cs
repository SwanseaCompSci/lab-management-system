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
    public sealed class TestsGetModuleSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleSpecification(moduleId: modules[0].Id);

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(modules[0].Id);
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetModuleSpecification(moduleId: Guid.Parse("a91f4de8-5f5c-47f5-9780-5378b76197af"));

            // Act
            var result = applicationDbContext.Modules.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
