using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Algorithms;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Allocation.Algorithms
{
    public class TestsFirstMatch
    {
        private IAllocator Allocator { get; } = new FirstMatch();

        [Test]
        public void Allocate_Without_PreAllocated_Labs_And_Module_Preferences()
        {
            // Arrange
            var programming1Id = Guid.Parse("d1f905b5-73c5-4f4d-ab4e-2575bde5d836");

            var labs = new List<LabModel>()
            {
                // CS-110
                new LabModel()
                {
                    Id = Guid.Parse("075c03e9-12f7-4f6f-95a0-46130edf9a89"),

                    ModuleId = programming1Id,
                    Level = Level.Year1,

                    Day = WorkDayOfWeek.Monday,
                    StartTime = new TimeOnly(12, 00),
                    EndTime = new TimeOnly(14, 00),
                    MinNumberOfStaff = 2,
                    MaxNumberOfStaff = 3,
                }, // 12:00 - 14:00 (2-3 staff)
                new LabModel()
                {
                    Id = Guid.Parse("0e7e327c-0f53-4339-9440-a33ea7c3d68f"),

                    ModuleId = programming1Id,
                    Level = Level.Year1,

                    Day = WorkDayOfWeek.Monday,
                    StartTime = new TimeOnly(14, 00),
                    EndTime = new TimeOnly(16, 00),
                    MinNumberOfStaff = 2,
                    MaxNumberOfStaff = 3,
                }, // 14:00 - 16:00 (2-3 staff)
            };

            var users = new List<UserModel>()
            {
                new UserModel()
                {
                    Id = Guid.Parse("81c713d5-92c3-49cb-91c1-ce7e2219ae21"),
                    AchievedLevel = Level.Year1,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a0ec5bf2-b247-4a10-bf32-04fe76d3b474"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("371571ed-0684-43d2-bde8-eca15cc11297"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("23f41b3d-3d19-4683-ab2e-55a72ed644e0"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a20e1159-1ae3-4c2b-a9a4-7f5beab0d71e"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        },
                    },
                },
                new UserModel()
                {
                    Id = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                    AchievedLevel = Level.Year2,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e16538cb-1ef7-4fe5-8de9-6afe4f362194"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a2790ccb-76bd-4052-b97a-3ac333a40b24"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(16, 00),
                            EndTime = new TimeOnly(17, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("ac203e7a-c41d-4c88-bad2-8288c1ecea94"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(17, 00),
                            EndTime = new TimeOnly(18, 00),
                            IsAllocated = false,
                        },
                    },
                },
                new UserModel()
                {
                    Id = Guid.Parse("a640d6df-d09f-402b-9864-7e65c15e81a0"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("7f07e452-1e4a-4e6b-bd2c-729d253cc2e8"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("06a989f2-431d-4172-9fcb-e36d5da7aeae"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e3012d49-ce20-42e7-b0cc-9f319dacf712"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        },
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("c1d9d768-4ee8-4594-b631-bcc91e5029cd"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        },
                    },
                },
            };

            var allocations = new List<AllocationModel>();

            // Act
            var result = Allocator.Allocate(users: users, labs: labs, allocations: allocations);

            // Assert
            result.Should().HaveCount(4);

            result.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();

            result.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
        }

        [Test]
        public void Allocate_Without_PreAllocated_Labs_But_With_Module_Preferences()
        {
            // Arrange
            var programming1Id = Guid.Parse("d1f905b5-73c5-4f4d-ab4e-2575bde5d836");
            var concurrencyId = Guid.Parse("6f631141-000a-44b6-8d06-e978062deb50");

            var labs = new List<LabModel>()
            {
                // CS-110
                new LabModel()
                {
                    Id = Guid.Parse("075c03e9-12f7-4f6f-95a0-46130edf9a89"),

                    ModuleId = programming1Id,
                    Level = Level.Year1,

                    Day = WorkDayOfWeek.Monday,
                    StartTime = new TimeOnly(12, 00),
                    EndTime = new TimeOnly(14, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 5,
                }, // Monday 12:00 - 14:00 (3-5 staff)
                new LabModel()
                {
                    Id = Guid.Parse("0e7e327c-0f53-4339-9440-a33ea7c3d68f"),

                    ModuleId = programming1Id,
                    Level = Level.Year1,

                    Day = WorkDayOfWeek.Monday,
                    StartTime = new TimeOnly(14, 00),
                    EndTime = new TimeOnly(16, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 5,
                }, // Monday 14:00 - 16:00 (3-5 staff)

                // CS-210
                new LabModel()
                {
                    Id = Guid.Parse("4da2d52c-d2eb-4eed-8dba-a2950c4ef162"),

                    ModuleId = concurrencyId,
                    Level = Level.Year2,

                    Day = WorkDayOfWeek.Thursday,
                    StartTime = new TimeOnly(10, 00),
                    EndTime = new TimeOnly(12, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 4,
                }, // Thursday 10:00 - 12:00 (3-4 staff)
                new LabModel()
                {
                    Id = Guid.Parse("d11fcf17-6c8b-493a-b876-dab7f7244697"),

                    ModuleId = concurrencyId,
                    Level = Level.Year2,

                    Day = WorkDayOfWeek.Thursday,
                    StartTime = new TimeOnly(12, 00),
                    EndTime = new TimeOnly(14, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 4,
                }, // Thursday 12:00 - 14:00 (3-4 staff)
            };

            var users = new List<UserModel>()
            {
                new UserModel()
                {
                    Id = Guid.Parse("81c713d5-92c3-49cb-91c1-ce7e2219ae21"),
                    AchievedLevel = Level.Year1,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>()
                    {
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("81c713d5-92c3-49cb-91c1-ce7e2219ae21"),
                            ModuleId = programming1Id,
                            Status = Status.Accepted,
                        }
                    },
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a0ec5bf2-b247-4a10-bf32-04fe76d3b474"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("371571ed-0684-43d2-bde8-eca15cc11297"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("23f41b3d-3d19-4683-ab2e-55a72ed644e0"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a20e1159-1ae3-4c2b-a9a4-7f5beab0d71e"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                    },
                }, // Year 1 - Allocated to Lab 1 and Lab 2
                new UserModel()
                {
                    Id = Guid.Parse("a78936d3-f38b-46e9-9139-8cd6fe7cd718"),
                    AchievedLevel = Level.Year2,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("4536e07d-9379-48de-9ddd-3c134b9eaef4"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("22a39ce8-5db2-43f5-890a-4c68f3f8489c"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e0cbe07b-457f-4c61-91c0-e26c840f59dd"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("426eadbe-3651-4354-a31e-8219895782dc"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                    },
                }, // Year 2 - Allocated to Lab 1 and Lab 2
                new UserModel()
                {
                    Id = Guid.Parse("a640d6df-d09f-402b-9864-7e65c15e81a0"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("7f07e452-1e4a-4e6b-bd2c-729d253cc2e8"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("06a989f2-431d-4172-9fcb-e36d5da7aeae"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e3012d49-ce20-42e7-b0cc-9f319dacf712"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("c1d9d768-4ee8-4594-b631-bcc91e5029cd"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                    },
                }, // Year 3 - Allocated to Lab 1 and Lab 2

                new UserModel()
                {
                    Id = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                    AchievedLevel = Level.Year2,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>()
                    {
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                            ModuleId = programming1Id,
                            Status = Status.Declined,
                        }, // Programming 1 - Declined
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                            ModuleId = concurrencyId,
                            Status = Status.Accepted
                        }, // Concurrency - Accepted
                    },
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("527ddc5e-801b-454a-8030-3754441def3b"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("0a0513b1-f055-4dc5-a787-ec8c8bfedac9"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a8cd5d86-802b-47a1-b9d6-c659c8fdf5cb"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e16538cb-1ef7-4fe5-8de9-6afe4f362194"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a2790ccb-76bd-4052-b97a-3ac333a40b24"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(16, 00),
                            EndTime = new TimeOnly(17, 00),
                            IsAllocated = false,
                        }, // Monday 16:00 - 17:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("ac203e7a-c41d-4c88-bad2-8288c1ecea94"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(17, 00),
                            EndTime = new TimeOnly(18, 00),
                            IsAllocated = false,
                        }, // Monday 17:00 - 18:00

                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("2a4b503a-4f57-4d1b-8bb0-c5848df8e14b"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(10, 00),
                            EndTime = new TimeOnly(11, 00),
                            IsAllocated = false,
                        }, // Thursday 10:00 - 11:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("fcca8566-dab2-48b3-8c43-a6a73e263613"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(11, 00),
                            EndTime = new TimeOnly(12, 00),
                            IsAllocated = false,
                        }, // Thursday 11:00 - 12:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("608036f4-f161-49f5-8cfd-0959e6242b51"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Thursday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("3618f16e-bff3-45f7-9f70-5248a91e3c65"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Thursday 13:00 - 14:00
                    },
                }, // Year 2 - Allocated to Lab 3 and Lab 4
                new UserModel()
                {
                    Id = Guid.Parse("82290a26-4103-4929-8ba0-25c59950b6f8"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>()
                    {
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("82290a26-4103-4929-8ba0-25c59950b6f8"),
                            ModuleId = concurrencyId,
                            Status = Status.Accepted
                        }, // Concurrency - Accepted
                    },
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("1850b6fa-f348-452d-b726-99b8193f9a64"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(10, 00),
                            EndTime = new TimeOnly(11, 00),
                            IsAllocated = false,
                        }, // Thursday 10:00 - 11:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("51d5abd6-c25b-4030-91f7-ebc92e6b2b37"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(11, 00),
                            EndTime = new TimeOnly(12, 00),
                            IsAllocated = false,
                        }, // Thursday 11:00 - 12:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("768a02ef-94d0-408c-8825-f65831140770"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Thursday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("8ff9900b-3697-42dd-8cfe-60aa180c690c"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Thursday 13:00 - 14:00
                    },
                }, // Year 3 - Allocated to Lab 3 and Lab 4
                new UserModel()
                {
                    Id = Guid.Parse("6775721b-db5d-4a2b-96ca-93912190d93c"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("83cd2182-2410-4818-85c1-af849c6a70e4"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(10, 00),
                            EndTime = new TimeOnly(11, 00),
                            IsAllocated = false,
                        }, // Thursday 10:00 - 11:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("506285a2-033d-4419-a1b8-1b8046cd20bc"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(11, 00),
                            EndTime = new TimeOnly(12, 00),
                            IsAllocated = false,
                        }, // Thursday 11:00 - 12:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("62a1b46c-4ad6-4e83-a95b-e3093eb3469b"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Thursday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("56fa8f66-4b9c-485e-bef4-29d54c249598"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Thursday 13:00 - 14:00
                    },
                }, // Year 3 - Allocated to Lab 3 and Lab 4
            };

            var allocations = new List<AllocationModel>();

            // Act
            var result = Allocator.Allocate(users: users, labs: labs, allocations: allocations);

            // Assert
            result.Should().HaveCount(12);

            result.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[1].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[1].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();

            result.Any(x => x.UserId == users[3].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[3].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[4].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[4].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[5].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[5].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
        }

        [Test]
        public void Allocate_With_PreAllocated_Labs_And_Module_Preferences()
        {
            // Arrange
            var programming1Id = Guid.Parse("d1f905b5-73c5-4f4d-ab4e-2575bde5d836");
            var concurrencyId = Guid.Parse("6f631141-000a-44b6-8d06-e978062deb50");

            var labs = new List<LabModel>()
            {
                // CS-110
                new LabModel()
                {
                    Id = Guid.Parse("075c03e9-12f7-4f6f-95a0-46130edf9a89"),

                    ModuleId = programming1Id,
                    Level = Level.Year1,

                    Day = WorkDayOfWeek.Monday,
                    StartTime = new TimeOnly(12, 00),
                    EndTime = new TimeOnly(14, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 5,
                }, // Monday 12:00 - 14:00 (3-5 staff)
                new LabModel()
                {
                    Id = Guid.Parse("0e7e327c-0f53-4339-9440-a33ea7c3d68f"),

                    ModuleId = programming1Id,
                    Level = Level.Year1,

                    Day = WorkDayOfWeek.Monday,
                    StartTime = new TimeOnly(14, 00),
                    EndTime = new TimeOnly(16, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 5,
                }, // Monday 14:00 - 16:00 (3-5 staff)

                // CS-210
                new LabModel()
                {
                    Id = Guid.Parse("4da2d52c-d2eb-4eed-8dba-a2950c4ef162"),

                    ModuleId = concurrencyId,
                    Level = Level.Year2,

                    Day = WorkDayOfWeek.Thursday,
                    StartTime = new TimeOnly(10, 00),
                    EndTime = new TimeOnly(12, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 4,
                }, // Thursday 10:00 - 12:00 (3-4 staff)
                new LabModel()
                {
                    Id = Guid.Parse("d11fcf17-6c8b-493a-b876-dab7f7244697"),

                    ModuleId = concurrencyId,
                    Level = Level.Year2,

                    Day = WorkDayOfWeek.Thursday,
                    StartTime = new TimeOnly(12, 00),
                    EndTime = new TimeOnly(14, 00),
                    MinNumberOfStaff = 3,
                    MaxNumberOfStaff = 4,
                }, // Thursday 12:00 - 14:00 (3-4 staff)
            };

            var users = new List<UserModel>()
            {
                new UserModel()
                {
                    Id = Guid.Parse("81c713d5-92c3-49cb-91c1-ce7e2219ae21"),
                    AchievedLevel = Level.Year1,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>()
                    {
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("81c713d5-92c3-49cb-91c1-ce7e2219ae21"),
                            ModuleId = programming1Id,
                            Status = Status.Accepted,
                        }
                    },
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a0ec5bf2-b247-4a10-bf32-04fe76d3b474"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("371571ed-0684-43d2-bde8-eca15cc11297"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("23f41b3d-3d19-4683-ab2e-55a72ed644e0"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a20e1159-1ae3-4c2b-a9a4-7f5beab0d71e"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                    },
                }, // Year 1 - Allocated to Lab 1 and Lab 2
                new UserModel()
                {
                    Id = Guid.Parse("a78936d3-f38b-46e9-9139-8cd6fe7cd718"),
                    AchievedLevel = Level.Year2,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("4536e07d-9379-48de-9ddd-3c134b9eaef4"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("22a39ce8-5db2-43f5-890a-4c68f3f8489c"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e0cbe07b-457f-4c61-91c0-e26c840f59dd"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("426eadbe-3651-4354-a31e-8219895782dc"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                    },
                }, // Year 2 - Allocated to Lab 1 and Lab 2
                new UserModel()
                {
                    Id = Guid.Parse("a640d6df-d09f-402b-9864-7e65c15e81a0"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("7f07e452-1e4a-4e6b-bd2c-729d253cc2e8"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("06a989f2-431d-4172-9fcb-e36d5da7aeae"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e3012d49-ce20-42e7-b0cc-9f319dacf712"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("c1d9d768-4ee8-4594-b631-bcc91e5029cd"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                    },
                }, // Year 3 - Allocated to Lab 1 and Lab 2

                new UserModel()
                {
                    Id = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                    AchievedLevel = Level.Year2,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>()
                    {
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                            ModuleId = programming1Id,
                            Status = Status.Declined,
                        }, // Programming 1 - Declined
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("d44b3cb6-a0a3-4753-855e-8fc684193e4e"),
                            ModuleId = concurrencyId,
                            Status = Status.Accepted
                        }, // Concurrency - Accepted
                    },
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("527ddc5e-801b-454a-8030-3754441def3b"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Monday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("0a0513b1-f055-4dc5-a787-ec8c8bfedac9"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Monday 13:00 - 14:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a8cd5d86-802b-47a1-b9d6-c659c8fdf5cb"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(14, 00),
                            EndTime = new TimeOnly(15, 00),
                            IsAllocated = false,
                        }, // Monday 14:00 - 15:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("e16538cb-1ef7-4fe5-8de9-6afe4f362194"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(15, 00),
                            EndTime = new TimeOnly(16, 00),
                            IsAllocated = false,
                        }, // Monday 15:00 - 16:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("a2790ccb-76bd-4052-b97a-3ac333a40b24"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(16, 00),
                            EndTime = new TimeOnly(17, 00),
                            IsAllocated = false,
                        }, // Monday 16:00 - 17:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("ac203e7a-c41d-4c88-bad2-8288c1ecea94"),

                            Day = WorkDayOfWeek.Monday,
                            StartTime = new TimeOnly(17, 00),
                            EndTime = new TimeOnly(18, 00),
                            IsAllocated = false,
                        }, // Monday 17:00 - 18:00

                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("2a4b503a-4f57-4d1b-8bb0-c5848df8e14b"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(10, 00),
                            EndTime = new TimeOnly(11, 00),
                            IsAllocated = false,
                        }, // Thursday 10:00 - 11:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("fcca8566-dab2-48b3-8c43-a6a73e263613"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(11, 00),
                            EndTime = new TimeOnly(12, 00),
                            IsAllocated = false,
                        }, // Thursday 11:00 - 12:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("608036f4-f161-49f5-8cfd-0959e6242b51"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Thursday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("3618f16e-bff3-45f7-9f70-5248a91e3c65"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Thursday 13:00 - 14:00
                    },
                }, // Year 2 - Allocated to Lab 3 and Lab 4
                new UserModel()
                {
                    Id = Guid.Parse("82290a26-4103-4929-8ba0-25c59950b6f8"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>()
                    {
                        new ModulePreferenceModel()
                        {
                            UserId = Guid.Parse("82290a26-4103-4929-8ba0-25c59950b6f8"),
                            ModuleId = concurrencyId,
                            Status = Status.Accepted
                        }, // Concurrency - Accepted
                    },
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("1850b6fa-f348-452d-b726-99b8193f9a64"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(10, 00),
                            EndTime = new TimeOnly(11, 00),
                            IsAllocated = false,
                        }, // Thursday 10:00 - 11:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("51d5abd6-c25b-4030-91f7-ebc92e6b2b37"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(11, 00),
                            EndTime = new TimeOnly(12, 00),
                            IsAllocated = false,
                        }, // Thursday 11:00 - 12:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("768a02ef-94d0-408c-8825-f65831140770"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Thursday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("8ff9900b-3697-42dd-8cfe-60aa180c690c"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Thursday 13:00 - 14:00
                    },
                }, // Year 3 - Allocated to Lab 3 and Lab 4
                new UserModel()
                {
                    Id = Guid.Parse("6775721b-db5d-4a2b-96ca-93912190d93c"),
                    AchievedLevel = Level.Year3,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>()
                    {
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("83cd2182-2410-4818-85c1-af849c6a70e4"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(10, 00),
                            EndTime = new TimeOnly(11, 00),
                            IsAllocated = false,
                        }, // Thursday 10:00 - 11:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("506285a2-033d-4419-a1b8-1b8046cd20bc"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(11, 00),
                            EndTime = new TimeOnly(12, 00),
                            IsAllocated = false,
                        }, // Thursday 11:00 - 12:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("62a1b46c-4ad6-4e83-a95b-e3093eb3469b"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(12, 00),
                            EndTime = new TimeOnly(13, 00),
                            IsAllocated = false,
                        }, // Thursday 12:00 - 13:00
                        new TimeAvailabilityModel()
                        {
                            Id = Guid.Parse("56fa8f66-4b9c-485e-bef4-29d54c249598"),

                            Day = WorkDayOfWeek.Thursday,
                            StartTime = new TimeOnly(13, 00),
                            EndTime = new TimeOnly(14, 00),
                            IsAllocated = false,
                        }, // Thursday 13:00 - 14:00
                    },
                }, // Year 3 - Allocated to Lab 3 and Lab 4

                new UserModel()
                {
                    Id = Guid.Parse("1183addd-91c0-4443-8147-3c63090e4fbe"),
                    AchievedLevel = Level.Masters,
                    MaxWeeklyWorkHours = 10,
                    ModulePreferences = new List<ModulePreferenceModel>(),
                    TimeAvailabilities = new List<TimeAvailabilityModel>(),
                }, // Master - Allocated to Lab 3 and Lab 4
            };

            var allocations = new List<AllocationModel>()
            {
                new AllocationModel(userId: users[0].Id, moduleId: programming1Id, labId: labs[0].Id), // Lab 1
                new AllocationModel(userId: users[0].Id, moduleId: programming1Id, labId: labs[1].Id), // Lab 2

                new AllocationModel(userId: users[6].Id, moduleId: concurrencyId, labId: labs[2].Id), // Lab 3
                new AllocationModel(userId: users[6].Id, moduleId: concurrencyId, labId: labs[3].Id), // Lab 4
            };

            // Act
            var result = Allocator.Allocate(users: users, labs: labs, allocations: allocations);

            // Assert
            result.Should().HaveCount(14);

            result.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[0].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[1].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[1].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[0].ModuleId && x.LabId == labs[0].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[2].Id && x.ModuleId == labs[1].ModuleId && x.LabId == labs[1].Id).Should().BeTrue();

            result.Any(x => x.UserId == users[3].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[3].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[4].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[4].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[5].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[5].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();

            result.Any(x => x.UserId == users[6].Id && x.ModuleId == labs[2].ModuleId && x.LabId == labs[2].Id).Should().BeTrue();
            result.Any(x => x.UserId == users[6].Id && x.ModuleId == labs[3].ModuleId && x.LabId == labs[3].Id).Should().BeTrue();
        }
    }
}
