using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.LabCommands
{
    public sealed class TestsCreateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var command = new Create.Command()
            {
                ModuleId = module.Id,
                Name = "Turring",
                Day = WorkDayOfWeek.Tuesday.ToString(),
                StartTime = new TimeOnly(10, 00).ToTimeSpan(),
                EndTime = new TimeOnly(12, 00).ToTimeSpan(),
                MinNumberOfStaff = 4,
                MaxNumberOfStaff = 5,
            };

            // Act
            var response = await Testing.SendAsync(command);

            // Assert
            response.Resource.Id.Should().NotBeEmpty();
            response.Resource.ModuleId.Should().Be(module.Id);
            response.Resource.Name.Should().Be(command.Name);
            response.Resource.Day.Should().Be(WorkDayOfWeek.Tuesday);
            response.Resource.StartTime.Should().Be(TimeOnly.FromTimeSpan(command.StartTime.Value));
            response.Resource.EndTime.Should().Be(TimeOnly.FromTimeSpan(command.EndTime.Value));
            response.Resource.MinNumberOfStaff.Should().Be(command.MinNumberOfStaff);
            response.Resource.MaxNumberOfStaff.Should().Be(command.MaxNumberOfStaff);
        }
    }
}
