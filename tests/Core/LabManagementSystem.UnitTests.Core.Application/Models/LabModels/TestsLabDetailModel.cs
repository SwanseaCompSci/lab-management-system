using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Linq;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Models.LabModels
{
    public class TestsLabDetailModel
    {
        [Test]
        public void TestMappingProfile()
        {
            var configuration = new MapperConfiguration(c =>
            {
                c.AddProfile<LabModelMappingProfile>();
                c.AddProfile<LabDetailModelMappingProfile>();

                c.AddProfile<UserModelMappingProfile>();
                c.AddProfile<LabScheduleModelMappingProfile>();
            });
            var mapper = new Mapper(configuration);

            var labEntity = new Lab(moduleId: Guid.NewGuid(),
                                    name: "Lab Name",
                                    day: WorkDayOfWeek.Wednesday,
                                    startTime: new TimeOnly(11, 30),
                                    endTime: new TimeOnly(13, 30),
                                    minNumberOfStaff: 1,
                                    maxNumberOfStaff: 2);
            var userEntity = new User(id: Guid.NewGuid(),
                                      firstName: "Jan",
                                      surname: "Kubiš",
                                      achievedLevel: Level.Year3,
                                      maxWeeklyWorkHours: 40);
            var userLabEntity = new UserLab(userId: userEntity.Id,
                                            labId: labEntity.Id);
            var labScheduleEntity = new LabSchedule(labId: labEntity.Id,
                                                    start: new DateTime(2023, 02, 08, 11, 30, 00),
                                                    end: new DateTime(2023, 02, 08, 13, 30, 00));

            labEntity.UserLabs.Add(userLabEntity);
            labEntity.LabSchedules.Add(labScheduleEntity);

            userEntity.UserLabs.Add(userLabEntity);

            userLabEntity.SetPrivatePropertyValue("User", userEntity);
            userLabEntity.SetPrivatePropertyValue("Lab", labEntity);

            labScheduleEntity.SetPrivatePropertyValue("Lab", labEntity);

            var model = mapper.Map<Lab, LabDetailModel>(labEntity);

            model.Id.Should().Be(labEntity.Id);
            model.ModuleId.Should().Be(labEntity.ModuleId);
            model.Name.Should().Be(labEntity.Name);
            model.Day.Should().Be(labEntity.Day);
            model.StartTime.Should().Be(labEntity.StartTime);
            model.EndTime.Should().Be(labEntity.EndTime);
            model.MinNumberOfStaff.Should().Be(labEntity.MinNumberOfStaff);
            model.MaxNumberOfStaff.Should().Be(labEntity.MaxNumberOfStaff);

            model.Users.Should().HaveCount(1);
            var userModel = model.Users.FirstOrDefault() ?? throw new NullReferenceException();
            userModel.Id.Should().Be(userEntity.Id);
            userModel.FirstName.Should().Be(userEntity.FirstName);
            userModel.Surname.Should().Be(userEntity.Surname);
            userModel.AchievedLevel.Should().Be(userEntity.AchievedLevel.ToString());
            userModel.MaxWeeklyWorkHours.Should().Be(userEntity.MaxWeeklyWorkHours);
            userModel.QuestionnaireToken.Should().Be(userEntity.QuestionnaireToken); ;

            model.LabSchedules.Should().HaveCount(1);
            var labScheduleModel = model.LabSchedules.FirstOrDefault() ?? throw new NullReferenceException();
            labScheduleModel.Id.Should().Be(labScheduleEntity.Id);
            labScheduleModel.LabId.Should().Be(labScheduleEntity.LabId);
            labScheduleModel.Start.Should().Be(labScheduleEntity.Start);
            labScheduleModel.End.Should().Be(labScheduleEntity.End);
        }
    }
}
