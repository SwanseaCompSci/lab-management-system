using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Models
{
    public class TestsLabModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<LabModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var moduleEntity = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);

            var labEntity = new Lab(moduleId: moduleEntity.Id,
                                    name: "Turring",
                                    day: WorkDayOfWeek.Wednesday,
                                    startTime: new TimeOnly(12, 00),
                                    endTime: new TimeOnly(13, 00),
                                    minNumberOfStaff: 4,
                                    maxNumberOfStaff: 5);
            labEntity.SetPrivatePropertyValue("Module", moduleEntity);
            moduleEntity.Labs.Add(labEntity);

            // Act
            var model = mapper.Map<Lab, LabModel>(labEntity);

            // Assert
            model.Id.Should().Be(labEntity.Id);

            model.ModuleId.Should().Be(moduleEntity.Id);
            model.Level.Should().Be(moduleEntity.Level);

            model.Day.Should().Be(labEntity.Day);
            model.StartTime.Should().Be(labEntity.StartTime);
            model.EndTime.Should().Be(labEntity.EndTime);

            model.MinNumberOfStaff.Should().Be(labEntity.MinNumberOfStaff);
            model.MaxNumberOfStaff.Should().Be(labEntity.MaxNumberOfStaff);

            model.AllocatedUsers.Should().BeEmpty();
        }
    }
}
