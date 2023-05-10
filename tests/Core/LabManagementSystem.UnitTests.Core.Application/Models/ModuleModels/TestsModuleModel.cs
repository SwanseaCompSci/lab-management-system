using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.ModuleModels
{
    public class TestsModuleModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ModuleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new Module(name: "Programming 2",
                                    code: "CS-115",
                                    level: Level.Year1);

            var model = mapper.Map<Module, ModuleModel>(entity);

            model.Id.Should().Be(entity.Id);
            model.Name.Should().Be(entity.Name);
            model.Code.Should().Be(entity.Code);
            model.Level.Should().Be(entity.Level.ToString());
        }
    }
}
