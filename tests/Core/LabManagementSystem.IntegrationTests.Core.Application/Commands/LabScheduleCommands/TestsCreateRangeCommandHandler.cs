using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.LabScheduleCommands
{
    public sealed class TestsCreateRangeCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-100", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            await Testing.AddAsync(entity: lab);

            var command = new CreateRange.Command()
            {
                LabId = lab.Id,
                StartDate = new DateTime(2020, 07, 01),
                NumberOfOccurrences = 3,
                Start = new TimeSpan(12, 00, 00),
                End = new TimeSpan(14, 00, 00),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(3);

            var firstStartDateTime = new DateTime(2020, 07, 01, 12, 00, 00);
            var firstEndDateTime = new DateTime(2020, 07, 01, 14, 00, 00);

            for (int i = 0; i < command.NumberOfOccurrences; i++)
            {
                var item = response.Resource
                    .FirstOrDefault(x => x.LabId == lab.Id && x.Start == firstStartDateTime.AddDays(i * 7) && x.End == firstEndDateTime.AddDays(i * 7));

                item.Should().NotBeNull();
            }
        }
    }
}
