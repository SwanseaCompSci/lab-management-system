using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.ModuleModels
{
    public class TestsModuleDetailUserRoleModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserModelMappingProfile>();
                c.AddProfile<ModuleDetailUserRoleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var userEntity = new User(id: Guid.NewGuid(),
                                      firstName: "Jozef",
                                      surname: "Gabčík",
                                      achievedLevel: Level.Year1,
                                      maxWeeklyWorkHours: 48);
            var userModuleEntity = new UserModule(userId: userEntity.Id,
                                                  moduleId: Guid.NewGuid(),
                                                  role: ModuleRole.LabCoordinator);
            userModuleEntity.SetPrivatePropertyValue("User", userEntity);

            var model = mapper.Map<UserModule, ModuleDetailUserRoleModel>(userModuleEntity);

            model.User.Id.Should().Be(userEntity.Id);
            model.User.FirstName.Should().Be(userEntity.FirstName);
            model.User.Surname.Should().Be(userEntity.Surname);
            model.User.AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            model.User.MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);
            model.User.QuestionnaireToken.Should().Be(userEntity.QuestionnaireToken);

            model.Role.Should().Be(userModuleEntity.Role.ToString());
        }
    }
}
