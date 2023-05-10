using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.LabModels
{
    public class TestsLabModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<LabModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new Lab(moduleId: Guid.NewGuid(),
                                 name: "Lab Name",
                                 day: WorkDayOfWeek.Wednesday,
                                 startTime: new TimeOnly(11, 30),
                                 endTime: new TimeOnly(13, 30),
                                 minNumberOfStaff: 1,
                                 maxNumberOfStaff: 2);

            var model = mapper.Map<Lab, LabModel>(entity);

            model.Id.Should().Be(entity.Id);
            model.ModuleId.Should().Be(entity.ModuleId);
            model.Name.Should().Be(entity.Name);
            model.Day.Should().Be(entity.Day);
            model.StartTime.Should().Be(entity.StartTime);
            model.EndTime.Should().Be(entity.EndTime);
            model.MinNumberOfStaff.Should().Be(entity.MinNumberOfStaff);
            model.MaxNumberOfStaff.Should().Be(entity.MaxNumberOfStaff);
        }
    }
}
