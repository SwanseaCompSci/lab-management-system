using Ardalis.Specification.EntityFrameworkCore;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Specifications.UserLabSpecifications;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Specifications.UserLabSpecifications
{
    public sealed class TestsGetUserLabsWhereModuleSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entities_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("546e75c1-568f-4448-939f-dc5d2356cce6"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: modules[0].Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),

                new Lab(moduleId: modules[1].Id, name: "Turring", day: WorkDayOfWeek.Thursday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
                new Lab(moduleId: modules[1].Id, name: "Lovelace", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: user.Id, labId: labs[0].Id),
                new UserLab(userId: user.Id, labId: labs[1].Id),

                new UserLab(userId: user.Id, labId: labs[2].Id),
                new UserLab(userId: user.Id, labId: labs[3].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserLabsWhereModuleSpecification(userId: user.Id, moduleId: modules[0].Id);

            // Act
            var result = applicationDbContext.UserLabs.WithSpecification(specification).ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Any(x => x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.LabId == labs[1].Id).Should().BeTrue();
        }

        [Test]
        public void Specified_Entities_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetUserLabsWhereModuleSpecification(userId: Guid.Parse("14558b49-e985-4378-8855-74e4ae8f443d"),
                                                                        moduleId: Guid.Parse("24b70e9f-34e1-4984-9df2-e3b067f23d5e"));

            // Act
            var result = applicationDbContext.UserLabs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
