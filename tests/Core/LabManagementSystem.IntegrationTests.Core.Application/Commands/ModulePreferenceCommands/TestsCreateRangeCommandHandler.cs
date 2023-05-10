using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.ModulePreferenceCommands
{
    public sealed class TestsCreateRangeCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var user = new User(id: Guid.Parse("ac263088-9cea-4eb1-bf1a-296bd73737d2"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year1, maxWeeklyWorkHours: 40);
            await Testing.AddAsync(entity: user);

            var modules = new List<Module>()
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),

                new Module(name: "Concepts of Computer Science 1", code: "CS-150", level: Level.Year1),
                new Module(name: "Concepts of Computer Science 2", code: "CS-155", level: Level.Year1),

                new Module(name: "Modelling Computing Systems 1", code: "CS-170", level: Level.Year1),
                new Module(name: "Modelling Computing Systems 2", code: "CS-175", level: Level.Year1),
            };
            await Testing.AddRangeAsync(entities: modules);

            var command = new CreateRange.Command()
            {
                UserId = user.Id,
                ModuleIds = modules.Select(x => x.Id),
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Should().HaveCount(6);
            foreach (var item in modules)
            {
                response.Resource.Any(x => x.ModuleId == item.Id).Should().BeTrue();
            }
        }
    }
}
