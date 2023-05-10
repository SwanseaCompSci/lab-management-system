using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.ModulePreferenceModels
{
    public class TestsModulePreferenceModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ModulePreferenceModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new ModulePreference(userId: Guid.NewGuid(),
                                              moduleId: Guid.NewGuid());

            var model = mapper.Map<ModulePreference, ModulePreferenceModel>(entity);

            model.UserId.Should().Be(entity.UserId);
            model.ModuleId.Should().Be(entity.ModuleId);
            model.Status.Should().Be(Status.PendingResponse.ToString());
        }
    }
}
