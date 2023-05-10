using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.TimeAvailabilityModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.TimeAvailabilityModels
{
    public class TestsTimeAvailabilityModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<TimeAvailabilityModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new TimeAvailability(userId: Guid.NewGuid(),
                                              day: WorkDayOfWeek.Thursday,
                                              startTime: new TimeOnly(16, 00),
                                              endTime: new TimeOnly(18, 00))
            {
                IsAllocated = true,
            };

            var model = mapper.Map<TimeAvailability, TimeAvailabilityModel>(entity);

            model.Id.Should().Be(entity.Id);
            model.UserId.Should().Be(entity.UserId);
            model.Day.Should().Be(entity.Day);
            model.StartTime.Should().Be(entity.StartTime);
            model.EndTime.Should().Be(entity.EndTime);
            model.IsAllocated.Should().Be(entity.IsAllocated);
        }
    }
}
