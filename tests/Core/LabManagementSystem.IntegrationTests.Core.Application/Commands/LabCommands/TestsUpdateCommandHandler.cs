using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using System;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Commands.LabCommands
{
    public sealed class TestsUpdateCommandHandler : TestBase
    {
        [Test]
        public async Task Handle_Command_Success()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);
            await Testing.AddAsync(entity: module);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Tuesday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(12, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            await Testing.AddAsync(entity: lab);

            var command = new Update.Command()
            {
                Id = lab.Id,
                ModuleId = lab.ModuleId,
                Name = "Lovelace",
                Day = WorkDayOfWeek.Wednesday.ToString(),
                StartTime = new TimeOnly(11, 00).ToTimeSpan(),
                EndTime = new TimeOnly(12, 00).ToTimeSpan(),
                MinNumberOfStaff = 5,
                MaxNumberOfStaff = 6,
            };

            // Act
            var result = await Testing.SendAsync(command);

            // Assert
            result.Resource.Should().NotBeNull();
            result.Resource.Id.Should().Be(command.Id);
            result.Resource.ModuleId.Should().Be(command.ModuleId);
            result.Resource.Name.Should().Be(command.Name);
            result.Resource.Day.Should().Be(WorkDayOfWeek.Wednesday);
            result.Resource.StartTime.Should().Be(new TimeOnly(command.StartTime.Value.Ticks));
            result.Resource.EndTime.Should().Be(new TimeOnly(command.EndTime.Value.Ticks));
            result.Resource.MinNumberOfStaff.Should().Be(command.MinNumberOfStaff);
            result.Resource.MaxNumberOfStaff.Should().Be(command.MaxNumberOfStaff);
        }

        [Test]
        public async Task Handle_Command_Throws_EntityNotFoundException()
        {
            // Arrange
            Testing.RunAsUser(user: Users.GetDefaultUser());

            var command = new Update.Command()
            {
                Id = Guid.Parse("4ef91fba-2a48-4b89-8b5b-53ca6765dc85"),
                ModuleId = Guid.Parse("9c8dbb3c-a80f-42c1-bf57-d68b2375ed69"),
                Name = "Lovelace",
                Day = WorkDayOfWeek.Wednesday.ToString(),
                StartTime = new TimeOnly(11, 00).ToTimeSpan(),
                EndTime = new TimeOnly(13, 00).ToTimeSpan(),
                MinNumberOfStaff = 5,
                MaxNumberOfStaff = 6,
            };

            // Act & Assert
            await FluentActions.Invoking(() => Testing.SendAsync(command))
                .Should().ThrowAsync<EntityNotFoundException>()
                .WithMessage($"Entity 'Lab' ({command.Id}) was not found.");
        }
    }
}
