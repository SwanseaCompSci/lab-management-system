using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Authorization.Requirements;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Authorization.Requirements
{
    public class TestsCurrentUserRequirement
    {
        private User AdministratorEntity { get; set; } = null!;
        private UserModel AdministratorModel { get; set; } = null!;
        private ClaimsPrincipal AdministratorClaimsPrincipal { get; set; } = null!;

        private User UserEntity { get; set; } = null!;
        private UserModel UserModel { get; set; } = null!;
        private ClaimsPrincipal UserClaimsPrincipal { get; set; } = null!;

        [OneTimeSetUp]
        public void Initialised()
        {
            AdministratorEntity = new User(id: Guid.Parse("688c852f-3124-45b3-8e08-5fff8f675322"),
                                     firstName: "Jan",
                                     surname: "Hrubý",
                                     achievedLevel: Level.Year3,
                                     maxWeeklyWorkHours: 40);
            AdministratorModel = new UserModel()
            {
                Id = AdministratorEntity.Id,
                FirstName = AdministratorEntity.FirstName,
                Surname = AdministratorEntity.Surname,
                AchievedLevel = AdministratorEntity.AchievedLevel.ToString(),
                MaxWeeklyWorkHours = AdministratorEntity.MaxWeeklyWorkHours,
                QuestionnaireToken = AdministratorEntity.QuestionnaireToken,
            };
            var administratorClaims = new List<Claim>()
            {
                new Claim(type: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                          value: "Administrator"),
                new Claim(type: "http://schemas.microsoft.com/identity/claims/objectidentifier",
                          value: AdministratorEntity.Id.ToString()),
                new Claim(type: "name",
                          value: $"{AdministratorEntity.FirstName} {AdministratorEntity.Surname}"),
            };
            AdministratorClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(administratorClaims));

            UserEntity = new User(id: Guid.Parse("fa1e33cd-b130-4991-9f0f-0db820082803"),
                firstName: "Josef",
                surname: "Valčík",
                achievedLevel: Level.Year2,
                maxWeeklyWorkHours: 10);
            var userClaims = new List<Claim>()
            {
                new Claim(type: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                          value: "User"),
                new Claim(type: "http://schemas.microsoft.com/identity/claims/objectidentifier",
                          value: UserEntity.Id.ToString()),
                new Claim(type: "name",
                          value: $"{UserEntity.FirstName} {UserEntity.Surname}"),
            };
            UserModel = new UserModel()
            {
                Id = UserEntity.Id,
                FirstName = UserEntity.FirstName,
                Surname = UserEntity.Surname,
                AchievedLevel = UserEntity.AchievedLevel.ToString(),
                MaxWeeklyWorkHours = UserEntity.MaxWeeklyWorkHours,
                QuestionnaireToken = UserEntity.QuestionnaireToken,
            };
            UserClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        }

        [Test]
        public async Task HandleRequirementAsync_UserEntity_User_Authorized()
        {
            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new CurrentUserRequirement() },
                                                          user: UserClaimsPrincipal,
                                                          resource: UserEntity);

            await new CurrentUserRequirementHandler().HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_UserEntity_User_Unauthorized()
        {
            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new CurrentUserRequirement() },
                                                          user: UserClaimsPrincipal,
                                                          resource: AdministratorEntity);

            await new CurrentUserRequirementHandler().HandleAsync(context);

            context.HasFailed.Should().BeTrue();
            context.FailureReasons.Count().Should().Be(1);

            var failureReason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            failureReason.Message.Should().Be($"User ({UserEntity.Id}) is not allowed to access information about User ({AdministratorEntity.Id}).");
        }

        [Test]
        public async Task HandleRequirementAsync_UserModel_User_Authorized()
        {
            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new CurrentUserRequirement() },
                                                          user: UserClaimsPrincipal,
                                                          resource: UserModel);

            await new CurrentUserModelRequirementHandler().HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_UserModel_User_Unauthorized()
        {
            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new CurrentUserRequirement() },
                                                          user: UserClaimsPrincipal,
                                                          resource: AdministratorModel);

            await new CurrentUserModelRequirementHandler().HandleAsync(context);

            context.HasFailed.Should().BeTrue();
            context.FailureReasons.Count().Should().Be(1);

            var failureReason = context.FailureReasons.FirstOrDefault() ?? throw new NullReferenceException();
            failureReason.Message.Should().Be($"User ({UserEntity.Id}) is not allowed to access information about User ({AdministratorEntity.Id}).");
        }

        [Test]
        public async Task HandleRequirementAsync_UserEntity_Administrator_Authorized()
        {
            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new CurrentUserRequirement() },
                                                          user: AdministratorClaimsPrincipal,
                                                          resource: UserEntity);

            await new CurrentUserRequirementHandler().HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
        [Test]
        public async Task HandleRequirementAsync_UserModel_Administrator_Authorized()
        {
            var context = new AuthorizationHandlerContext(requirements: new IAuthorizationRequirement[] { new CurrentUserRequirement() },
                                                          user: AdministratorClaimsPrincipal,
                                                          resource: UserModel);

            await new CurrentUserModelRequirementHandler().HandleAsync(context);

            context.HasSucceeded.Should().BeTrue();
        }
    }
}
