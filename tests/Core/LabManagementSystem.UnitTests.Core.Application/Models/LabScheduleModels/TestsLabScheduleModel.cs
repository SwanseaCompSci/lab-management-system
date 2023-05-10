using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.LabScheduleModels
{
    public class TestsLabScheduleModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<LabScheduleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new LabSchedule(labId: Guid.NewGuid(),
                                         start: new DateTime(2020, 05, 30, 12, 00, 00),
                                         end: new DateTime(2020, 05, 30, 14, 00, 00));

            var model = mapper.Map<LabSchedule, LabScheduleModel>(entity);

            model.Id.Should().Be(entity.Id);
            model.LabId.Should().Be(entity.LabId);
            model.Start.Should().Be(entity.Start);
            model.End.Should().Be(entity.End);
        }
    }
}
