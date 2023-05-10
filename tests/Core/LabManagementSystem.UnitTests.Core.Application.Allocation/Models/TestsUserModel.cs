using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Models
{
    public class TestsUserModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserModelMappingProfile>();
                c.AddProfile<TimeAvailabilityModelMappingProfile>();
                c.AddProfile<ModulePreferenceModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new User(id: Guid.Parse("0570a104-f2e3-4004-ba94-5f2cc4da9383"),
                                  firstName: "Anna",
                                  surname: "Hunt",
                                  achievedLevel: Level.Year3,
                                  maxWeeklyWorkHours: 10);
            entity.TimeAvailabilities.Add(new TimeAvailability(userId: entity.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00)));
            entity.TimeAvailabilities.Add(new TimeAvailability(userId: entity.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(14, 00)));
            entity.TimeAvailabilities.Add(new TimeAvailability(userId: entity.Id, day: WorkDayOfWeek.Monday, startTime: new TimeOnly(14, 00), endTime: new TimeOnly(15, 00)));

            entity.ModulePreferences.Add(new ModulePreference(userId: entity.Id, moduleId: Guid.Parse("f103956c-28e5-407b-bc93-63161c439d9d")) { Status = Status.Accepted });
            entity.ModulePreferences.Add(new ModulePreference(userId: entity.Id, moduleId: Guid.Parse("fc20bac5-8102-43cf-992a-2e3cb03c8c17")) { Status = Status.Declined });
            entity.ModulePreferences.Add(new ModulePreference(userId: entity.Id, moduleId: Guid.Parse("0d671d37-d27a-4497-b977-9850e5caffa6")) { Status = Status.PendingResponse });

            // Act
            var model = mapper.Map<User, UserModel>(entity);

            // Assert
            model.Id.Should().Be(entity.Id);
            model.AchievedLevel.Should().Be(entity.AchievedLevel);
            model.MaxWeeklyWorkHours.Should().Be(entity.MaxWeeklyWorkHours);

            model.TimeAvailabilities.Should().HaveCount(3);
            foreach (var item in entity.TimeAvailabilities)
            {
                model.TimeAvailabilities.Any(x => x.Id == item.Id && x.Day == item.Day && x.StartTime == item.StartTime && x.EndTime == item.EndTime && x.IsAllocated == false).Should().BeTrue();
            }

            model.ModulePreferences.Should().HaveCount(3);
            foreach (var item in entity.ModulePreferences)
            {
                model.ModulePreferences.Any(x => x.UserId == item.UserId && x.ModuleId == item.ModuleId && x.Status == item.Status).Should().BeTrue();
            }
        }
    }
}
