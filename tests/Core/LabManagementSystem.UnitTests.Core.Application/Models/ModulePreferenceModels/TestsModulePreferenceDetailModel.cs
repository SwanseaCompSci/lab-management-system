using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.ModulePreferenceModels
{
    public class TestsModulePreferenceDetailModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ModulePreferenceDetailModelMappingProfile>();
                c.AddProfile<ModuleModelMappingProfile>();
                c.AddProfile<UserModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var userEntity = new User(id: Guid.Parse("cfbd818d-279e-4692-bc8f-517c3bf90330"),
                                      firstName: "Mike",
                                      surname: "Ross",
                                      achievedLevel: Level.Year3,
                                      maxWeeklyWorkHours: 40);

            var moduleEntity = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);

            var modulePreferenceEntity = new ModulePreference(userId: userEntity.Id, moduleId: moduleEntity.Id);
            userEntity.ModulePreferences.Add(modulePreferenceEntity);
            moduleEntity.ModulePreferences.Add(modulePreferenceEntity);
            modulePreferenceEntity.SetPrivatePropertyValue("User", userEntity);
            modulePreferenceEntity.SetPrivatePropertyValue("Module", moduleEntity);

            // Act
            var model = mapper.Map<ModulePreference, ModulePreferenceDetailModel>(modulePreferenceEntity);

            // Assert
            model.User.Id.Should().Be(userEntity.Id);
            model.User.FirstName.Should().Be(userEntity.FirstName);
            model.User.Surname.Should().Be(userEntity.Surname);
            model.User.AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            model.User.MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);

            model.Module.Id.Should().Be(moduleEntity.Id);
            model.Module.Name.Should().Be(moduleEntity.Name);
            model.Module.Code.Should().Be(moduleEntity.Code);
            model.Module.Level.Should().Be(moduleEntity.Level.ToString());

            model.Status.Should().Be(Status.PendingResponse.ToString());
        }
    }
}
