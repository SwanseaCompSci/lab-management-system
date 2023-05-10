using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.IntegrationTests.Core.Application.Allocation
{
    internal static class Users
    {
        internal static ClaimsPrincipal GetDefaultUser() => GetAdministrator();
        internal static ClaimsPrincipal GetAdministrator()
        {
            var claims = new List<Claim>
            {
                new Claim(type: "uid", value: "be78ecda-a0ee-42fa-afcb-1bd7670da93a"),
                new Claim(type: "name", value: "Administrator User"),
                new Claim(type: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", value: "Administrator"),
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
        internal static ClaimsPrincipal GetUser(Guid id)
        {
            var claims = new List<Claim>
            {
                new Claim(type: "uid", value: id.ToString()),
                new Claim(type: "name", value: "Normal User"),
                new Claim(type: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", value: "User"),
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
