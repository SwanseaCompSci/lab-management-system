using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Linq;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.ModuleModels
{
    public class TestsModuleDetailModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ModuleModelMappingProfile>();
                c.AddProfile<ModuleDetailModelMappingProfile>();
                c.AddProfile<ModuleDetailUserRoleModelMappingProfile>();
                c.AddProfile<LabModelMappingProfile>();
                c.AddProfile<UserModelMappingProfile>();
                c.AddProfile<ModulePreferenceDetailModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var moduleEntity = new Module(name: "Programming 2",
                                          code: "CS-115",
                                          level: Level.Year1);

            var labEntity = new Lab(moduleId: moduleEntity.Id,
                                    name: "Lab Name",
                                    day: WorkDayOfWeek.Tuesday,
                                    startTime: new TimeOnly(10, 00),
                                    endTime: new TimeOnly(12, 00),
                                    minNumberOfStaff: 4,
                                    maxNumberOfStaff: 5);
            moduleEntity.Labs.Add(labEntity);

            var userEntity = new User(id: Guid.NewGuid(),
                                      firstName: "Marie",
                                      surname: "Kovárníková",
                                      achievedLevel: Level.Masters,
                                      maxWeeklyWorkHours: 20);

            var userModuleEntity = new UserModule(userId: userEntity.Id,
                                                  moduleId: moduleEntity.Id,
                                                  role: ModuleRole.TeachingAssistant);
            moduleEntity.UserModules.Add(userModuleEntity);
            userModuleEntity.SetPrivatePropertyValue("User", userEntity);
            userModuleEntity.SetPrivatePropertyValue("Module", moduleEntity);

            var modulePreferenceEntity = new ModulePreference(userId: userEntity.Id, moduleId: moduleEntity.Id);
            moduleEntity.ModulePreferences.Add(modulePreferenceEntity);
            userEntity.ModulePreferences.Add(modulePreferenceEntity);
            modulePreferenceEntity.SetPrivatePropertyValue("User", userEntity);
            modulePreferenceEntity.SetPrivatePropertyValue("Module", moduleEntity);

            // Act
            var moduleModel = mapper.Map<Module, ModuleDetailModel>(moduleEntity);

            // Assert
            moduleModel.Id.Should().Be(moduleEntity.Id);
            moduleModel.Name.Should().Be(moduleEntity.Name);
            moduleModel.Code.Should().Be(moduleEntity.Code);
            moduleModel.Level.Should().Be(moduleEntity.Level.ToString());

            moduleModel.Labs.Should().HaveCount(1);
            var labModel = moduleModel.Labs.FirstOrDefault() ?? throw new NullReferenceException();

            labModel.Id.Should().Be(labEntity.Id);
            labModel.ModuleId.Should().Be(labEntity.ModuleId);
            labModel.Name.Should().Be(labEntity.Name);
            labModel.Day.Should().Be(labEntity.Day);
            labModel.StartTime.Should().Be(labEntity.StartTime);
            labModel.EndTime.Should().Be(labEntity.EndTime);
            labModel.MinNumberOfStaff.Should().Be(labEntity.MinNumberOfStaff);
            labModel.MaxNumberOfStaff.Should().Be(labEntity.MaxNumberOfStaff);

            moduleModel.UserRoles.Should().HaveCount(1);
            var userRoleModel = moduleModel.UserRoles.FirstOrDefault() ?? throw new NullReferenceException();

            userRoleModel.User.Id.Should().Be(userEntity.Id);
            userRoleModel.User.FirstName.Should().Be(userEntity.FirstName);
            userRoleModel.User.Surname.Should().Be(userEntity.Surname);
            userRoleModel.User.AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            userRoleModel.User.MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);
            userRoleModel.User.QuestionnaireToken.Should().Be(userEntity.QuestionnaireToken);
            userRoleModel.Role.Should().Be(userModuleEntity.Role.ToString());

            moduleModel.ModulePreferences.Should().HaveCount(1);
            var modulePreferencesModel = moduleModel.ModulePreferences.FirstOrDefault() ?? throw new NullReferenceException();

            modulePreferencesModel.User.Id.Should().Be(userEntity.Id);
            modulePreferencesModel.User.FirstName.Should().Be(userEntity.FirstName);
            modulePreferencesModel.User.Surname.Should().Be(userEntity.Surname);
            modulePreferencesModel.User.AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            modulePreferencesModel.User.MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);
            modulePreferencesModel.User.QuestionnaireToken.Should().Be(userEntity.QuestionnaireToken);

            modulePreferencesModel.Module.Id.Should().Be(moduleEntity.Id);
            modulePreferencesModel.Module.Name.Should().Be(moduleEntity.Name);
            modulePreferencesModel.Module.Code.Should().Be(moduleEntity.Code);
            modulePreferencesModel.Module.Level.Should().Be(moduleEntity.Level.ToString());

            modulePreferencesModel.Status.Should().Be(Status.PendingResponse.ToString());
        }
    }
}
