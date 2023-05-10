using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.AllocationModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Linq;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.AllocationModels
{
    public class TestsAllocationResultModel
    {
        [Test]
        public void TestMappingProfile()
        {
            // Arrange
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<AllocationResultModelMappingProfile>();
                c.AddProfile<LabModelMappingProfile>();
                c.AddProfile<ModuleModelMappingProfile>();
                c.AddProfile<UserModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var moduleEntity = new Module(name: "Programming 1", code: "CS-110", level: Level.Year1);

            var labEntity = new Lab(moduleId: moduleEntity.Id, name: "Group 1", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(13, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5);
            labEntity.SetPrivatePropertyValue("Module", moduleEntity);
            moduleEntity.Labs.Add(labEntity);

            var userEntity = new User(id: Guid.Parse("56541076-6d60-4e5d-99a0-4faff3282ec6"), firstName: "Mike", surname: "Ross", achievedLevel: Level.Year2, maxWeeklyWorkHours: 10);

            var userLabEntity = new UserLab(userId: userEntity.Id, labId: labEntity.Id);
            userLabEntity.SetPrivatePropertyValue("User", userEntity);
            userLabEntity.SetPrivatePropertyValue("Lab", labEntity);
            userEntity.UserLabs.Add(userLabEntity);
            labEntity.UserLabs.Add(userLabEntity);

            // Act
            var model = mapper.Map<Lab, AllocationResultModel>(labEntity);

            // Assert
            model.Lab.Id.Should().Be(labEntity.Id);
            model.Lab.ModuleId.Should().Be(labEntity.ModuleId);
            model.Lab.Name.Should().Be(labEntity.Name);
            model.Lab.Day.Should().Be(labEntity.Day);
            model.Lab.StartTime.Should().Be(labEntity.StartTime);
            model.Lab.EndTime.Should().Be(labEntity.EndTime);
            model.Lab.MinNumberOfStaff.Should().Be(labEntity.MinNumberOfStaff);
            model.Lab.MaxNumberOfStaff.Should().Be(labEntity.MaxNumberOfStaff);

            model.Module.Id.Should().Be(moduleEntity.Id);
            model.Module.Name.Should().Be(moduleEntity.Name);
            model.Module.Code.Should().Be(moduleEntity.Code);
            model.Module.Level.Should().Be(moduleEntity.Level.ToString());

            model.AllocatedUsers.Should().HaveCount(1);
            model.AllocatedUsers.First().Id.Should().Be(userEntity.Id);
            model.AllocatedUsers.First().FirstName.Should().Be(userEntity.FirstName);
            model.AllocatedUsers.First().Surname.Should().Be(userEntity.Surname);
            model.AllocatedUsers.First().AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            model.AllocatedUsers.First().MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);
            model.AllocatedUsers.First().QuestionnaireToken.Should().Be(userEntity.QuestionnaireToken);
        }
    }
}
