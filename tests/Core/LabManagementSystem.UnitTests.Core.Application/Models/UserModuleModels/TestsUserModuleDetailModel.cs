using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.UserModuleModels
{
    public class TestsUserModuleDetailModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserModuleDetailModelMappingProfile>();
                c.AddProfile<UserModuleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var userEntity = new User(id: Guid.NewGuid(),
                                      firstName: "Josef",
                                      surname: "Valčík",
                                      achievedLevel: Level.Year2,
                                      maxWeeklyWorkHours: 25);
            var moduleEntity = new Module(name: "Computer Graphics",
                                          code: "CS-255",
                                          level: Level.Year2);

            var userModuleEntity = new UserModule(userId: userEntity.Id,
                                                  moduleId: Guid.NewGuid(),
                                                  role: ModuleRole.TeachingAssistant);
            userModuleEntity.SetPrivatePropertyValue("User", userEntity);
            userModuleEntity.SetPrivatePropertyValue("Module", moduleEntity);

            userEntity.UserModules.Add(userModuleEntity);
            moduleEntity.UserModules.Add(userModuleEntity);

            var model = mapper.Map<UserModule, UserModuleDetailModel>(userModuleEntity);

            model.UserId.Should().Be(userModuleEntity.UserId);
            model.ModuleId.Should().Be(userModuleEntity.ModuleId);
            model.Role.Should().Be(userModuleEntity.Role.ToString());

            model.UserFirstName.Should().Be(userEntity.FirstName);
            model.UserSurname.Should().Be(userEntity.Surname);
            model.ModuleName.Should().Be(moduleEntity.Name);
        }
    }
}
