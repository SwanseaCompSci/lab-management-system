using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.LabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.LabSpecifications
{
    public sealed class TestsGetLabSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[1].Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabSpecification(labId: labs[0].Id);

            // Act
            var response = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            response.Should().HaveCount(1);
            response[0].Id.Should().Be(labs[0].Id);
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabSpecification(labId: Guid.Parse("fe98f6fd-7f0b-48a0-9575-e0730b26fa69"));

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
