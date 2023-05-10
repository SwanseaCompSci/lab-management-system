using FluentAssertions;
using NUnit.Framework;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using System.Collections.Generic;
using System.Security.Claims;
using AppAuthorization = SwanseaCompSci.LabManagementSystem.Core.Application.Authorization;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application.Authorization
{
    public class TestsHelpers
    {
        private ClaimsPrincipal ClaimsPrincipal { get; set; } = null!;

        [OneTimeSetUp]
        public void Initialize()
        {
            var claims = new List<Claim>()
            {
                new Claim(type: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                          value: "Administrator"),
                new Claim(type: "http://schemas.microsoft.com/identity/claims/objectidentifier",
                          value: "fa1e33cd-b130-4991-9f0f-0db820082803"),
                new Claim(type: "name",
                          value: "Josef Valčík"),
            };
            var identity = new ClaimsIdentity(claims);

            ClaimsPrincipal = new ClaimsPrincipal(identity);
        }

        [Test]
        public void Test_GetApplicationRole()
        {
            var appRole = AppAuthorization.Helpers.GetApplicationRole(user: ClaimsPrincipal);

            appRole!.Value.Should().Be(ApplicationRole.Administrator);
        }

        [Test]
        public void Test_GetUserId()
        {
            var userId = AppAuthorization.Helpers.GetUserId(user: ClaimsPrincipal);

            userId!.Value.Should().Be("fa1e33cd-b130-4991-9f0f-0db820082803");
        }

        [Test]
        public void Test_GetUserName()
        {
            var userName = AppAuthorization.Helpers.GetUserName(user: ClaimsPrincipal);

            userName.Should().Be("Josef Valčík");
        }

        [Test]
        public void Test_GetUserFirstName()
        {
            var firstName = AppAuthorization.Helpers.GetUserFirstName(user: ClaimsPrincipal);

            firstName.Should().Be("Josef");
        }

        [Test]
        public void Test_GetUserSurname()
        {
            var surname = AppAuthorization.Helpers.GetUserSurname(user: ClaimsPrincipal);

            surname.Should().Be("Valčík");
        }
    }
}
