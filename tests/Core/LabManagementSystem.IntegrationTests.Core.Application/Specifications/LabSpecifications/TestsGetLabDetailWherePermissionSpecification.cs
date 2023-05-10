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
    public sealed class TestsGetLabDetailWherePermissionSpecification : TestBase
    {
        [Test]
        public async Task Specified_Entity_Found_Has_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("eadf8158-609d-4fa3-a904-03073e675041"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
                new User(id: Guid.Parse("b86f2be5-641e-4625-91fa-9222bf22dda7"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
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
            };
            await Testing.AddRangeAsync(entities: userModules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Lovelace", day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 06, 12, 00, 00), end: new DateTime(2023, 02, 06, 13, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 13, 12, 00, 00), end: new DateTime(2023, 02, 13, 13, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 20, 12, 00, 00), end: new DateTime(2023, 02, 20, 13, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 27, 12, 00, 00), end: new DateTime(2023, 02, 27, 13, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabDetailWherePermissionSpecification(labId: labs[0].Id, userId: users[0].Id);

            // Act
            var response = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            response.Should().HaveCount(1);
            response[0].Id.Should().Be(labs[0].Id);
        }

        [Test]
        public async Task Specified_Entity_Found_Does_Not_Have_Permission()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("eadf8158-609d-4fa3-a904-03073e675041"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30),
                new User(id: Guid.Parse("b86f2be5-641e-4625-91fa-9222bf22dda7"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
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
            };
            await Testing.AddRangeAsync(entities: userModules);

            var labs = new List<Lab>()
            {
                new Lab(moduleId: modules[0].Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
                new Lab(moduleId: modules[0].Id, name: "Lovelace", day: WorkDayOfWeek.Wednesday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            await Testing.AddRangeAsync(entities: labs);

            var userLabs = new List<UserLab>()
            {
                new UserLab(userId: users[0].Id, labId: labs[0].Id),
            };
            await Testing.AddRangeAsync(entities: userLabs);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 06, 12, 00, 00), end: new DateTime(2023, 02, 06, 13, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 13, 12, 00, 00), end: new DateTime(2023, 02, 13, 13, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 20, 12, 00, 00), end: new DateTime(2023, 02, 20, 13, 00, 00)),
                new LabSchedule(labId: labs[0].Id, start: new DateTime(2023, 02, 27, 12, 00, 00), end: new DateTime(2023, 02, 27, 13, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabDetailWherePermissionSpecification(labId: labs[0].Id, userId: users[1].Id);

            // Act
            var response = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            response.Should().BeEmpty();
        }

        [Test]
        public void Specified_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var applicationDbContext = Testing.GetService<IApplicationDbContext>() ?? throw new NullReferenceException();

            var specification = new GetLabDetailWherePermissionSpecification(labId: Guid.Parse("fea09db0-6968-4404-9bf5-7f6877307886"),
                                                                             userId: Guid.Parse("ca6ed156-9609-4e2c-9cbf-7d23671f326c"));

            // Act
            var result = applicationDbContext.Labs.WithSpecification(specification).ToList();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
