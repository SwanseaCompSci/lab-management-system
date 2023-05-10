using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.UserModels
{
    public class TestsUserDetailModulePreferenceModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserDetailModulePreferenceModelMappingProfile>();
                c.AddProfile<ModuleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var moduleEntity = new Module(name: "Computer Graphics",
                                          code: "CS-255",
                                          level: Level.Year2);
            var modulePreferenceEntity = new ModulePreference(userId: Guid.NewGuid(),
                                                              moduleId: moduleEntity.Id)
            {
                Status = Status.Accepted,
            };
            modulePreferenceEntity.SetPrivatePropertyValue("Module", moduleEntity);

            var model = mapper.Map<ModulePreference, UserDetailModulePreferenceModel>(modulePreferenceEntity);

            model.Module.Id.Should().Be(moduleEntity.Id);
            model.Module.Name.Should().Be(moduleEntity.Name);
            model.Module.Code.Should().Be(moduleEntity.Code);
            model.Module.Level.Should().Be(moduleEntity.Level.ToString());

            model.Status.Should().Be(modulePreferenceEntity.Status.ToString());
        }
    }
}
