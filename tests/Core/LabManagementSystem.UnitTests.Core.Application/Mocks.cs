using Microsoft.EntityFrameworkCore;
using Moq;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application
{
    internal static class Mocks
    {
        internal static IDateTimeService GetDateTimeServiceMock()
        {
            var mock = new Mock<IDateTimeService>();
            mock.SetupGet(d => d.Today).Returns(new DateTime(2020, 01, 01));
            mock.SetupGet(d => d.UtcNow).Returns(new DateTime(2020, 01, 01, 12, 00, 00, DateTimeKind.Utc));
            return mock.Object;
        }

        internal static IApplicationDbContext GetApplicationDbContext(
            IReadOnlyList<Module>? modules = null,
            IReadOnlyList<ModulePreference>? modulePreferences = null,
            IReadOnlyList<Lab>? labs = null,
            IReadOnlyList<LabSchedule>? labSchedules = null,

            IReadOnlyList<UserModule>? userModules = null,
            IReadOnlyList<UserLab>? userLabs = null,
            IReadOnlyList<UserLabSchedule>? userLabSchedules = null,

            IReadOnlyList<User>? users = null,
            IReadOnlyList<TimeAvailability>? timeAvailabilities = null)
        {
            var mock = new Mock<IApplicationDbContext>();
            mock.SetupGet(d => d.Modules).Returns(Create(modules ?? Array.Empty<Module>()).Object);
            mock.SetupGet(d => d.ModulePreferences).Returns(Create(modulePreferences ?? Array.Empty<ModulePreference>()).Object);
            mock.SetupGet(d => d.Labs).Returns(Create(labs ?? Array.Empty<Lab>()).Object);
            mock.SetupGet(d => d.LabSchedules).Returns(Create(labSchedules ?? Array.Empty<LabSchedule>()).Object);

            mock.SetupGet(d => d.UserModules).Returns(Create(userModules ?? Array.Empty<UserModule>()).Object);
            mock.SetupGet(d => d.UserLabs).Returns(Create(userLabs ?? Array.Empty<UserLab>()).Object);
            mock.SetupGet(d => d.UserLabSchedules).Returns(Create(userLabSchedules ?? Array.Empty<UserLabSchedule>()).Object);

            mock.SetupGet(d => d.Users).Returns(Create(users ?? Array.Empty<User>()).Object);
            mock.SetupGet(d => d.TimeAvailabilities).Returns(Create(timeAvailabilities ?? Array.Empty<TimeAvailability>()).Object);
            return mock.Object;
        }

        // Creates a mock DbSet from the specified data.
        internal static Mock<DbSet<T>> Create<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mock = new Mock<DbSet<T>>();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mock;
        }
    }
}
