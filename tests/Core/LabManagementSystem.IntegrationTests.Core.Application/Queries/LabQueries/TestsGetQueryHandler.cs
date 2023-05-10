using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.LabQueries
{
    public sealed class TestsGetQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3);
            await Testing.AddAsync(entity: lab);

            var query = new Get.Query(labId: lab.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(lab.Id);
            response.Resource.Name.Should().Be(lab.Name);
            response.Resource.Day.Should().Be(lab.Day);
            response.Resource.StartTime.Should().Be(lab.StartTime);
            response.Resource.EndTime.Should().Be(lab.EndTime);
            response.Resource.MinNumberOfStaff.Should().Be(lab.MinNumberOfStaff);
            response.Resource.MaxNumberOfStaff.Should().Be(lab.MaxNumberOfStaff);
        }

        [Test]
        public async Task Handle_Query_As_Administrator_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetAdministrator());

            var query = new Get.Query(labId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Has_Permission()
        {
            // Arrange
            var userId = Guid.Parse("f5850762-f26f-4071-bb9f-4bd4b8f2e0c3");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            var user = new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var userModule = new UserModule(userId: user.Id, moduleId: module.Id, role: ModuleRole.TeachingAssistant);
            await Testing.AddAsync(entity: userModule);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3);
            await Testing.AddAsync(entity: lab);

            var query = new Get.Query(labId: lab.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(lab.Id);
            response.Resource.Name.Should().Be(lab.Name);
            response.Resource.Day.Should().Be(lab.Day);
            response.Resource.StartTime.Should().Be(lab.StartTime);
            response.Resource.EndTime.Should().Be(lab.EndTime);
            response.Resource.MinNumberOfStaff.Should().Be(lab.MinNumberOfStaff);
            response.Resource.MaxNumberOfStaff.Should().Be(lab.MaxNumberOfStaff);
        }

        [Test]
        public async Task Handle_Query_As_User_Entity_Found_Does_Not_Have_Permission()
        {
            var userId = Guid.Parse("f5850762-f26f-4071-bb9f-4bd4b8f2e0c3");
            Testing.RunAsUser(user: Users.GetUser(id: userId));

            var user = new User(id: userId, firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20);
            await Testing.AddAsync(entity: user);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3);
            await Testing.AddAsync(entity: lab);

            var query = new Get.Query(labId: lab.Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
