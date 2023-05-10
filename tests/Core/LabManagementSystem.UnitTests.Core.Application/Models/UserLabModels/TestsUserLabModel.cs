using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserLabModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.UserLabModels
{
    public class TestsUserLabModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserLabModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new UserLab(userId: Guid.NewGuid(),
                                     labId: Guid.NewGuid());

            var model = mapper.Map<UserLab, UserLabModel>(entity);

            model.UserId.Should().Be(entity.UserId);
            model.LabId.Should().Be(entity.LabId);
        }
    }
}
