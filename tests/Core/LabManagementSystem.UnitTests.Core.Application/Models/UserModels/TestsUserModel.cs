using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.UserModels
{
    public class TestsUserModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<UserModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var entity = new User(id: Guid.NewGuid(),
                                  firstName: "Lenka",
                                  surname: "Fafková",
                                  achievedLevel: Level.PhD,
                                  maxWeeklyWorkHours: 40)
            {
                QuestionnaireToken = Guid.NewGuid(),
            };

            var model = mapper.Map<User, UserModel>(entity);

            model.Id.Should().Be(entity.Id);
            model.FirstName.Should().Be(entity.FirstName);
            model.Surname.Should().Be(entity.Surname);
            model.AchievedLevel.Should().Be(entity.AchievedLevel.ToString());
            model.MaxWeeklyWorkHours.Should().Be(entity.MaxWeeklyWorkHours);
            model.QuestionnaireToken.Should().Be(entity.QuestionnaireToken);
        }
    }
}
