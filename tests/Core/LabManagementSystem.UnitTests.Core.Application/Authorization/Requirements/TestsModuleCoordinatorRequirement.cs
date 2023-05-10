using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Authorization.Requirements;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Authorization.Requirements
{
    public class TestsModuleCoordinatorRequirement
    {
        [Test]
        public async Task HandleRequirementAsync_Administrator_ModuleEntity_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };

            var dbContext = Mocks.GetApplicationDbContext(modules: modules, users: users);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], true);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: modules[0]);

            await new ModuleCoordinatorModuleRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_Administrator_ModuleModel_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var moduleModel = new ModuleModel()
            {
                Id = modules[0].Id,
                Name = modules[0].Name,
                Code = modules[0].Code,
                Level = modules[0].Level.ToString(),
            };

            var dbContext = Mocks.GetApplicationDbContext(modules: modules, users: users);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], true);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: moduleModel);

            await new ModuleCoordinatorModuleModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_Administrator_LabEntity_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var labs = new Lab[]
            {
                new Lab(moduleId: Guid.NewGuid(), name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(12, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, labs: labs);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], true);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labs[0]);

            await new ModuleCoordinatorLabRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_Administrator_LabModel_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var labs = new Lab[]
            {
                new Lab(moduleId: Guid.NewGuid(), name: "Turring", day: WorkDayOfWeek.Monday, startTime: new TimeOnly(10, 00), endTime: new TimeOnly(12, 00), minNumberOfStaff: 4, maxNumberOfStaff: 5),
            };
            var labModel = new LabModel()
            {
                Id = labs[0].Id,
                ModuleId = labs[0].ModuleId,
                Name = labs[0].Name,
                Day = labs[0].Day,
                StartTime = labs[0].StartTime,
                EndTime = labs[0].EndTime,
                MinNumberOfStaff = labs[0].MinNumberOfStaff,
                MaxNumberOfStaff = labs[0].MaxNumberOfStaff,
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, labs: labs);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], true);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labModel);

            await new ModuleCoordinatorLabModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }

        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_ModuleEntity_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: modules[0]);

            await new ModuleCoordinatorModuleRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_ModuleEntity_Unauthorized()
        {
            // Module Coordinator for a different module

            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: modules[1]);

            await new ModuleCoordinatorModuleRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[1].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_ModuleModel_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var moduleModel = new ModuleModel()
            {
                Id = modules[0].Id,
                Name = modules[0].Name,
                Code = modules[0].Code,
                Level = modules[0].Level.ToString(),
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: moduleModel);

            await new ModuleCoordinatorModuleModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_ModuleModel_Unauthorized()
        {
            // Module Coordinator for a different module

            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var moduleModel = new ModuleModel()
            {
                Id = modules[1].Id,
                Name = modules[1].Name,
                Code = modules[1].Code,
                Level = modules[1].Level.ToString(),
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: moduleModel);

            await new ModuleCoordinatorModuleModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[1].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_LabEntity_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[0].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[0]);
            modules[0].Labs.Add(labs[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labs[0]);

            await new ModuleCoordinatorLabRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_LabEntity_Unauthorized()
        {
            // Module Coordinator for a different module

            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[1].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[1]);
            modules[1].Labs.Add(labs[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labs[0]);

            await new ModuleCoordinatorLabRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[1].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_LabModel_Authorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[0].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[0]);
            modules[0].Labs.Add(labs[0]);

            var labModel = new LabModel()
            {
                Id = labs[0].Id,
                ModuleId = labs[0].ModuleId,
                Name = labs[0].Name,
                Day = labs[0].Day,
                StartTime = labs[0].StartTime,
                EndTime = labs[0].EndTime,
                MinNumberOfStaff = labs[0].MinNumberOfStaff,
                MaxNumberOfStaff = labs[0].MaxNumberOfStaff,
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labModel);

            await new ModuleCoordinatorLabModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_ModuleCoordinator_LabModel_Unauthorized()
        {
            // Module Coordinator for a different module

            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
                new Module(name: "Programming 2", code: "CS-115", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.ModuleCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[1].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[1]);
            modules[1].Labs.Add(labs[0]);

            var labModel = new LabModel()
            {
                Id = labs[0].Id,
                ModuleId = labs[0].ModuleId,
                Name = labs[0].Name,
                Day = labs[0].Day,
                StartTime = labs[0].StartTime,
                EndTime = labs[0].EndTime,
                MinNumberOfStaff = labs[0].MinNumberOfStaff,
                MaxNumberOfStaff = labs[0].MaxNumberOfStaff,
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labModel);

            await new ModuleCoordinatorLabModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[1].Id}).");
        }

        [Test]
        public async Task HandleRequirementAsync_LabCoordinator_ModuleEntity_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.LabCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: modules[0]);

            await new ModuleCoordinatorModuleRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_LabCoordinator_ModuleModel_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.LabCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var moduleModel = new ModuleModel()
            {
                Id = modules[0].Id,
                Name = modules[0].Name,
                Code = modules[0].Code,
                Level = modules[0].Level.ToString(),
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: moduleModel);

            await new ModuleCoordinatorModuleModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_LabCoordinator_LabEntity_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.LabCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[0].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[0]);
            modules[0].Labs.Add(labs[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labs[0]);

            await new ModuleCoordinatorLabRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_LabCoordinator_LabModel_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };
            var userModules = new UserModule[]
            {
                new UserModule(userId: users[0].Id, moduleId: modules[0].Id, role: ModuleRole.LabCoordinator),
            };
            userModules[0].SetPrivatePropertyValue("User", users[0]);
            userModules[0].SetPrivatePropertyValue("Module", modules[0]);
            users[0].UserModules.Add(userModules[0]);
            modules[0].UserModules.Add(userModules[0]);

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[0].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[0]);
            modules[0].Labs.Add(labs[0]);

            var labModel = new LabModel()
            {
                Id = labs[0].Id,
                ModuleId = labs[0].ModuleId,
                Name = labs[0].Name,
                Day = labs[0].Day,
                StartTime = labs[0].StartTime,
                EndTime = labs[0].EndTime,
                MinNumberOfStaff = labs[0].MinNumberOfStaff,
                MaxNumberOfStaff = labs[0].MaxNumberOfStaff,
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs, userModules: userModules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labModel);

            await new ModuleCoordinatorLabModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }

        [Test]
        public async Task HandleRequirementAsync_User_ModuleEntity_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: modules[0]);

            await new ModuleCoordinatorModuleRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_User_ModuleModel_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };

            var moduleModel = new ModuleModel()
            {
                Id = modules[0].Id,
                Name = modules[0].Name,
                Code = modules[0].Code,
                Level = modules[0].Level.ToString(),
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: moduleModel);

            await new ModuleCoordinatorModuleModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_User_LabEntity_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[0].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[0]);
            modules[0].Labs.Add(labs[0]);

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labs[0]);

            await new ModuleCoordinatorLabRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
        [Test]
        public async Task HandleRequirementAsync_User_LabModel_Unauthorized()
        {
            var users = new User[]
            {
                new User(id: Guid.Parse("ca9ee2de-762c-4a72-8961-8cf18e4df54f"), firstName: "Jozef", surname: "Gabčík", achievedLevel: Level.PhD, maxWeeklyWorkHours: 48),
            };
            var modules = new Module[]
            {
                new Module(name: "Programming 1", code: "CS-110", level: Level.Year1),
            };

            var labs = new Lab[]
            {
                new Lab(moduleId: modules[0].Id,
                        name: "Turring",
                        day: WorkDayOfWeek.Wednesday,
                        startTime: new TimeOnly(10, 00),
                        endTime: new TimeOnly(12, 00),
                        minNumberOfStaff: 4,
                        maxNumberOfStaff: 5),
            };
            labs[0].SetPrivatePropertyValue("Module", modules[0]);
            modules[0].Labs.Add(labs[0]);

            var labModel = new LabModel()
            {
                Id = labs[0].Id,
                ModuleId = labs[0].ModuleId,
                Name = labs[0].Name,
                Day = labs[0].Day,
                StartTime = labs[0].StartTime,
                EndTime = labs[0].EndTime,
                MinNumberOfStaff = labs[0].MinNumberOfStaff,
                MaxNumberOfStaff = labs[0].MaxNumberOfStaff,
            };

            var dbContext = Mocks.GetApplicationDbContext(users: users, modules: modules, labs: labs);
            var claimsPrincipal = Helpers.GetClaimsPrincipal(users[0], false);

            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new ModuleCoordinatorRequirement() },
                user: claimsPrincipal,
                resource: labModel);

            await new ModuleCoordinatorLabModelRequirementHandler(dbContext).HandleAsync(context);

            context.HasFailed.Should().BeTrue();

            context.FailureReasons.Should().HaveCount(1);
            var reason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            reason.Message.Should().Be($"User (ca9ee2de-762c-4a72-8961-8cf18e4df54f) does not have a ModuleCoordinator role for Module ({modules[0].Id}).");
        }
    }
}
