using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using AppCommon = SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Common
{
    public class TestsHelpers
    {
        [Test]
        public void IsAvailable_User_Is_Available()
        {
            // Arrange
            var user = new UserModel()
            {
                Id = Guid.Parse("e832853a-5ed4-45c5-b56b-3ce6b8966087"),
                AchievedLevel = Level.Year3,
                MaxWeeklyWorkHours = 10,
                TimeAvailabilities = new List<TimeAvailabilityModel>()
                {
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("0f66ebcd-6025-4407-a9db-cc298c208ce5"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(11, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5286f19d-296b-4214-a329-1fdda45ded1c"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(11, 00),
                        EndTime = new TimeOnly(12, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5687764e-5c0b-4ff7-81b8-c7834e1301d0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(12, 00),
                        EndTime = new TimeOnly(13, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("a668c111-7a44-413b-be11-267af3ef99b0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(13, 00),
                        EndTime = new TimeOnly(14, 00),
                        IsAllocated = false,
                    },
                }
            };

            // Act
            var result = AppCommon.Helpers.IsAvailable(user, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00));

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void IsAvailable_User_Is_Not_Available_For_The_First_Hour()
        {
            // Arrange
            var user = new UserModel()
            {
                Id = Guid.Parse("e832853a-5ed4-45c5-b56b-3ce6b8966087"),
                AchievedLevel = Level.Year3,
                MaxWeeklyWorkHours = 10,
                TimeAvailabilities = new List<TimeAvailabilityModel>()
                {
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("0f66ebcd-6025-4407-a9db-cc298c208ce5"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(11, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5286f19d-296b-4214-a329-1fdda45ded1c"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(11, 00),
                        EndTime = new TimeOnly(12, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5687764e-5c0b-4ff7-81b8-c7834e1301d0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(12, 00),
                        EndTime = new TimeOnly(13, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("a668c111-7a44-413b-be11-267af3ef99b0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(13, 00),
                        EndTime = new TimeOnly(14, 00),
                        IsAllocated = false,
                    },
                }
            };

            // Act
            var result = AppCommon.Helpers.IsAvailable(user, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(09, 00), endTime: new TimeOnly(11, 00));

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void IsAvailable_User_Is_Not_Available_For_The_Second_Hour()
        {
            // Arrange
            var user = new UserModel()
            {
                Id = Guid.Parse("e832853a-5ed4-45c5-b56b-3ce6b8966087"),
                AchievedLevel = Level.Year3,
                MaxWeeklyWorkHours = 10,
                TimeAvailabilities = new List<TimeAvailabilityModel>()
                {
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("0f66ebcd-6025-4407-a9db-cc298c208ce5"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(11, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5286f19d-296b-4214-a329-1fdda45ded1c"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(11, 00),
                        EndTime = new TimeOnly(12, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5687764e-5c0b-4ff7-81b8-c7834e1301d0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(12, 00),
                        EndTime = new TimeOnly(13, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("a668c111-7a44-413b-be11-267af3ef99b0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(13, 00),
                        EndTime = new TimeOnly(14, 00),
                        IsAllocated = false,
                    },

                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("070b538b-4dd9-4376-979c-476a1d6a79c4"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(14, 00),
                        EndTime = new TimeOnly(15, 00),
                        IsAllocated = true,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("a1a9d1f8-6dc8-430d-92fe-d3d9056477f8"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(15, 00),
                        EndTime = new TimeOnly(16, 00),
                        IsAllocated = true,
                    },
                }
            };

            // Act
            var result = AppCommon.Helpers.IsAvailable(user, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(13, 00), endTime: new TimeOnly(15, 00));

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void AllocateHours_Success()
        {
            // Arrange
            var user = new UserModel()
            {
                Id = Guid.Parse("e832853a-5ed4-45c5-b56b-3ce6b8966087"),
                AchievedLevel = Level.Year3,
                MaxWeeklyWorkHours = 10,
                TimeAvailabilities = new List<TimeAvailabilityModel>()
                {
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("0f66ebcd-6025-4407-a9db-cc298c208ce5"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(10, 00),
                        EndTime = new TimeOnly(11, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5286f19d-296b-4214-a329-1fdda45ded1c"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(11, 00),
                        EndTime = new TimeOnly(12, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("5687764e-5c0b-4ff7-81b8-c7834e1301d0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(12, 00),
                        EndTime = new TimeOnly(13, 00),
                        IsAllocated = false,
                    },
                    new TimeAvailabilityModel()
                    {
                        Id = Guid.Parse("a668c111-7a44-413b-be11-267af3ef99b0"),
                        Day = WorkDayOfWeek.Friday,
                        StartTime = new TimeOnly(13, 00),
                        EndTime = new TimeOnly(14, 00),
                        IsAllocated = false,
                    },
                }
            };

            // Act
            AppCommon.Helpers.AllocateHours(user: user, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00));

            // Assert
            user.TimeAvailabilities
                .Any(x => x.StartTime >= new TimeOnly(12, 00)
                       && x.EndTime <= new TimeOnly(14, 00)
                       && x.IsAllocated == false)
                .Should()
                .BeFalse();
        }

        [Test]
        public void AllocateHours_Failure_Hour_Cannot_Be_Allocated()
        {
            // Arrange
            var user = new UserModel()
            {
                Id = Guid.Parse("e832853a-5ed4-45c5-b56b-3ce6b8966087"),
                AchievedLevel = Level.Year3,
                MaxWeeklyWorkHours = 10,
            };

            // Act
            AppCommon.Helpers.AllocateHours(user: user, day: WorkDayOfWeek.Friday, startTime: new TimeOnly(12, 00), endTime: new TimeOnly(14, 00));

            // Act & Assert
            user.TimeAvailabilities.Should().HaveCount(2);

            user.TimeAvailabilities.Any(x => x.Id != Guid.Empty
                                          && x.Day == WorkDayOfWeek.Friday
                                          && x.StartTime == new TimeOnly(12, 00)
                                          && x.EndTime == new TimeOnly(13, 00)
                                          && x.IsAllocated == true).Should().BeTrue();

            user.TimeAvailabilities.Any(x => x.Id != Guid.Empty
                                          && x.Day == WorkDayOfWeek.Friday
                                          && x.StartTime == new TimeOnly(13, 00)
                                          && x.EndTime == new TimeOnly(14, 00)
                                          && x.IsAllocated == true).Should().BeTrue();
        }
    }
}
