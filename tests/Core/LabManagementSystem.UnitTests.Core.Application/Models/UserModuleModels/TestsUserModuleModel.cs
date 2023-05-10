using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.UserModuleModels
{
    public class TestsUserModuleModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserModuleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new UserModule(userId: Guid.NewGuid(),
                                        moduleId: Guid.NewGuid(),
                                        role: ModuleRole.TeachingAssistant);

            var model = mapper.Map<UserModule, UserModuleModel>(entity);

            model.UserId.Should().Be(entity.UserId);
            model.ModuleId.Should().Be(entity.ModuleId);
            model.Role.Should().Be(entity.Role.ToString());
        }
    }
}
