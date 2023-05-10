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
    public sealed class TestsDeleteAllCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var users = new List<User>()
            {
                new User(id: Guid.Parse("75867ee3-4cad-464c-9383-638d2e7b34b4"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year3, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("23f05ab5-1fdf-47c9-b85f-298e6fe47179"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10),
            };
            await Testing.AddRangeAsync(entities: users);

            var timeAvailabilities = new List<TimeAvailability>()
            {
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)),
                new TimeAvailability(userId: users[0].Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(15, 00), endTime: new TimeOnly(16, 00)),

                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)),
                new TimeAvailability(userId: users[1].Id, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)),
            };
            await Testing.AddRangeAsync(entities: timeAvailabilities);

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(6);
            foreach (var item in timeAvailabilities)
            {
                response.Resource!.Any(x => x.UserId == item.UserId && x.Day == item.Day && x.StartTime == item.StartTime && x.EndTime == item.EndTime).Should().BeTrue();
            }
        }

        [Test]
        public async Task Handle_Command_No_Resources_To_Delete()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().BeNull();
        }
    }
}
