using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.DashboardModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.DashboardModels
{
    public class TestsWorkShiftModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<WorkShiftModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var module = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);

            var lab = new Lab(moduleId: module.Id, name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(15, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            lab.SetPrivatePropertyValue("Module", module);
            module.Labs.Add(lab);

            var labSchedule = new LabSchedule(labId: lab.Id, start: new DateTime(2023, 02, 06, 13, 00, 00), end: new DateTime(2023, 02, 06, 15, 00, 00));
            labSchedule.SetPrivatePropertyValue("Lab", lab);
            lab.LabSchedules.Add(labSchedule);

            var userLabSchedule = new UserLabSchedule(userId: Guid.Parse("9684b536-b3a7-4cc8-80e5-76b9ec5d3a3c"), labScheduleId: labSchedule.Id);
            userLabSchedule.SetPrivatePropertyValue("LabSchedule", labSchedule);
            labSchedule.UserLabSchedules.Add(userLabSchedule);

            // Act
            var model = mapper.Map<UserLabSchedule, WorkShiftModel>(userLabSchedule);

            // Assert
            model.LabScheduleId.Should().Be(labSchedule.Id);
            model.LabScheduleStart.Should().Be(labSchedule.Start);
            model.LabScheduleEnd.Should().Be(labSchedule.End);

            model.LabId.Should().Be(lab.Id);
            model.LabName.Should().Be(lab.Name);

            model.ModuleId.Should().Be(module.Id);
            model.ModuleName.Should().Be(module.Name);
        }
    }
}
