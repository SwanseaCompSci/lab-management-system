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
    public sealed class TestsGetLabWherePermissionSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found_Has_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("d6d6d45c-3c46-4ac0-911f-60c69a2f5014"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var userModule = new UserModule(userId: user.Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant);
            await Testing.AddAsync(entity: userModule);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabWherePermissionSpecification(labId: labs[0].Id, userId: user.Id);

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Id.Should().Be(labs[0].Id);
        }

        [Test]
        public async Task Specified_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("d6d6d45c-3c46-4ac0-911f-60c69a2f5014"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: module.Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabWherePermissionSpecification(labId: labs[1].Id, userId: user.Id);

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabWherePermissionSpecification(labId: Guid.Parse("95cc8080-0696-4d70-9cb6-90919ba111c1"),
                                                                       userId: Guid.Parse("aaea92dd-5e19-425f-b080-2ec10e93f5e6"));

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
