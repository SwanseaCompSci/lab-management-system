using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Models
{
    public class TestsTimeAvailabilityModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<TimeAvailabilityModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new TimeAvailability(userId: Guid.Parse("937a3519-6a9e-4fdb-8603-1f015f014ea8"),
                                              day: WorkDayOfWeek.Monday,
                                              startTime: new TimeOnly(12, 00),
                                              endTime: new TimeOnly(13, 00));

            // Act
            var model = mapper.Map<TimeAvailability, TimeAvailabilityModel>(entity);

            // Assert
            model.Id.Should().Be(entity.Id);
            model.Day.Should().Be(entity.Day);
            model.StartTime.Should().Be(entity.StartTime);
            model.EndTime.Should().Be(entity.EndTime);
            model.IsAllocated.Should().Be(entity.IsAllocated);
        }
    }
}
