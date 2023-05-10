using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.TimeAvailabilityCommands
{
    public sealed class TestsDeleteRangeCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("23f05ab5-1fdf-47c9-b85f-298e6fe47179"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year3, maxWeeklyWorkHours: 30);
            await Testing.AddAsync(entity: user);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(16, 00), endTime: new TimeOnly(17, 00)),
                new TimeAvailability(userId: user.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(17, 00), endTime: new TimeOnly(18, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var command = new DeleteRange.Command(userId: user.Id);

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().NotBeNull();
            response.Resource.Should().HaveCount(6);
            foreach (var item in timeAvailabilities)
            {
                response.Resource!.Any(x => x.UserId == item.UserId && x.StartTime == item.StartTime && x.EndTime == item.EndTime).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Command_No_Resources_To_Delete()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new DeleteRange.Command(userId: Guid.NewGuid());

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
