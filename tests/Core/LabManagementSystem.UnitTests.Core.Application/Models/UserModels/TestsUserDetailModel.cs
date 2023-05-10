using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.TimeAvailabilityModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Linq;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.UserModels
{
    public class TestsUserDetailModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserDetailModelMappingProfile>();
                c.AddProfile<UserModelMappingProfile>();

                c.AddProfile<UserDetailModulePreferenceModelMappingProfile>();
                c.AddProfile<ModuleModelMappingProfile>();

                c.AddProfile<UserDetailModuleRoleModelMappingProfile>();
                c.AddProfile<TimeAvailabilityModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var userEntity = new User(id: Guid.NewGuid(),
                                      firstName: "Adolf",
                                      surname: "Opálka",
                                      achievedLevel: Level.Year2,
                                      maxWeeklyWorkHours: 10);
            var moduleEntity = new Module(name: "Writing Mobile Apps",
                                          code: "CSC306",
                                          level: Level.Year3);
            var modulePreferenceEntity = new ModulePreference(userId: userEntity.Id,
                                                              moduleId: moduleEntity.Id)
            {
                Status = Status.Declined,
            };
            modulePreferenceEntity.SetPrivatePropertyValue("User", userEntity);
            modulePreferenceEntity.SetPrivatePropertyValue("Module", moduleEntity);

            userEntity.ModulePreferences.Add(modulePreferenceEntity);

            var userModuleEntity = new UserModule(userId: userEntity.Id,
                                                  moduleId: moduleEntity.Id,
                                                  role: ModuleRole.LabCoordinator);
            userModuleEntity.SetPrivatePropertyValue("User", userEntity);
            userModuleEntity.SetPrivatePropertyValue("Module", moduleEntity);

            userEntity.UserModules.Add(userModuleEntity);

            var timeAvailabilityEntity = new TimeAvailability(userId: userEntity.Id,
                                                              day: WorkDayOfWeek.Wednesday,
                                                              startTime: new TimeOnly(13, 00),
                                                              endTime: new TimeOnly(15, 00))
            {
                IsAllocated = true,
            };
            timeAvailabilityEntity.SetPrivatePropertyValue("User", userEntity);

            userEntity.TimeAvailabilities.Add(timeAvailabilityEntity);

            var model = mapper.Map<User, UserDetailModel>(userEntity);

            model.Id.Should().Be(userEntity.Id);
            model.FirstName.Should().Be(userEntity.FirstName);
            model.Surname.Should().Be(userEntity.Surname);
            model.AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            model.MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);
            model.QuestionnaireToken.Should().Be(userEntity.QuestionnaireToken);

            model.ModulePreferences.Should().HaveCount(1);
            var modulePreferenceModel = model.ModulePreferences.FirstOrDefault() ?? throw new NullReferenceException();

            modulePreferenceModel.Module.Id.Should().Be(moduleEntity.Id);
            modulePreferenceModel.Module.Name.Should().Be(moduleEntity.Name);
            modulePreferenceModel.Module.Code.Should().Be(moduleEntity.Code);
            modulePreferenceModel.Module.Level.Should().Be(moduleEntity.Level.ToString());
            modulePreferenceModel.Status.Should().Be(Status.Declined.ToString());

            model.ModuleRoles.Should().HaveCount(1);
            var moduleRoleModel = model.ModuleRoles.FirstOrDefault() ?? throw new NullReferenceException();

            moduleRoleModel.Module.Id.Should().Be(moduleEntity.Id);
            moduleRoleModel.Module.Name.Should().Be(moduleEntity.Name);
            moduleRoleModel.Module.Code.Should().Be(moduleEntity.Code);
            moduleRoleModel.Module.Level.Should().Be(moduleEntity.Level.ToString());
            moduleRoleModel.Role.Should().Be(ModuleRole.LabCoordinator.ToString());

            model.TimeAvailabilities.Should().HaveCount(1);
            var timeAvailabilitiesModel = model.TimeAvailabilities.FirstOrDefault() ?? throw new NullReferenceException();

            timeAvailabilitiesModel.Id.Should().Be(timeAvailabilityEntity.Id);
            timeAvailabilitiesModel.UserId.Should().Be(timeAvailabilityEntity.UserId);
            timeAvailabilitiesModel.Day.Should().Be(timeAvailabilityEntity.Day);
            timeAvailabilitiesModel.StartTime.Should().Be(timeAvailabilityEntity.StartTime);
            timeAvailabilitiesModel.EndTime.Should().Be(timeAvailabilityEntity.EndTime);
            timeAvailabilitiesModel.IsAllocated.Should().Be(timeAvailabilityEntity.IsAllocated);
        }
    }
}
