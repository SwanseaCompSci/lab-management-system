using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabScheduleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Queries.LabScheduleQueries
{
    public sealed class TestsGetQueryHandler : TestBase
    {
        [Test]
        public async Task Handle_Query_Entity_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 2, maxNumberOfStaff: 3);
            await Testing.AddAsync(entity: lab);

            var labSchedules = new List<LabSchedule>()
            {
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 06, 12, 00, 00), end: new DateTime(2023, 02, 06, 13, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 13, 12, 00, 00), end: new DateTime(2023, 02, 13, 13, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 20, 12, 00, 00), end: new DateTime(2023, 02, 20, 13, 00, 00)),
                new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 27, 12, 00, 00), end: new DateTime(2023, 02, 27, 13, 00, 00)),
            };
            await Testing.AddRangeAsync(entities: labSchedules);

            var query = new Get.Query(labScheduleId: labSchedules[1].Id);

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource!.Id.Should().Be(labSchedules[1].Id);
            response.Resource.Start.Should().Be(labSchedules[1].Start);
            response.Resource.End.Should().Be(labSchedules[1].End);
        }

        [Test]
        public async Task Handle_Query_Entity_Not_Found()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var query = new Get.Query(labScheduleId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(query);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
