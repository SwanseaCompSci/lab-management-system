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
    public sealed class TestsCreateRangeCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("23f05ab5-1fdf-47c9-b85f-298e6fe47179"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 20)
            {
                QuestionnaireToken = Guid.Parse("a4d10e99-80bc-4e20-bc13-48b43b3c3285"),
            };
            await Testing.AddAsync(entity: user);

            var command = new CreateRange.Command()
            {
                UserId = user.Id,
                Token = user.QuestionnaireToken ?? throw new NullReferenceException(),
                TimeAvailabilities = new List<CreateRange.TimeAvailabilityCommandModel>()
                {
                    new() { Day = WorkDayOfWeek.Monday.ToString(), StartTime = new TimeOnly(12, 00), EndTime = new TimeOnly(13, 00), },
                    new() { Day = WorkDayOfWeek.Monday.ToString(), StartTime = new TimeOnly(13, 00), EndTime = new TimeOnly(14, 00), },
                    new() { Day = WorkDayOfWeek.Monday.ToString(), StartTime = new TimeOnly(14, 00), EndTime = new TimeOnly(15, 00), },
                    new() { Day = WorkDayOfWeek.Monday.ToString(), StartTime = new TimeOnly(15, 00), EndTime = new TimeOnly(16, 00), },
                    new() { Day = WorkDayOfWeek.Monday.ToString(), StartTime = new TimeOnly(16, 00), EndTime = new TimeOnly(17, 00), },
                    new() { Day = WorkDayOfWeek.Monday.ToString(), StartTime = new TimeOnly(17, 00), EndTime = new TimeOnly(18, 00), },
                },
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(6);
            foreach (var item in command.TimeAvailabilities)
            {
                response.Resource.Any(x => x.UserId == user.Id && x.StartTime == item.StartTime && x.EndTime == item.EndTime).Should().BeTrue();
            }
        }
    }
}
