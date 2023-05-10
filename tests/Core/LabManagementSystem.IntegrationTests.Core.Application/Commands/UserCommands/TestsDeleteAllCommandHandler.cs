using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.UserCommands
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
                new User(id: Guid.Parse("0a4bd2a5-1e8d-4d39-a7fb-37a319f45417"), firstName: "Anna", surname: "Hunt", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
                new User(id: Guid.Parse("fea9cb8a-5929-4bd7-8204-489cdb8760f4"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 10),
            };
            await Testing.AddRangeAsync(entities: users);

            var command = new DeleteAll.Command();

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(2);
            foreach (var item in users)
            {
                response.Resource!.Any(x => x.Id == item.Id).Should().BeTrue();
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
