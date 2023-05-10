using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Models
{
    public class TestsModulePreferenceModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<ModulePreferenceModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new ModulePreference(userId: Guid.Parse("31f3e810-7c02-4bb9-85f6-eece3572b6d4"),
                                              moduleId: Guid.Parse("12114da1-8bbc-432a-907f-ee44a63aae1e"));

            // Act
            var model = mapper.Map<ModulePreference, ModulePreferenceModel>(entity);

            // Assert
            model.UserId.Should().Be(entity.UserId);
            model.ModuleId.Should().Be(entity.ModuleId);
            model.Status.Should().Be(entity.Status);
        }
    }
}
