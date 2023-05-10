using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Authorization
{
    public static class Helpers
    {
        public static ApplicationRole? GetApplicationRole(ClaimsPrincipal user)
        {
            var claimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

            var orderedAppClaims = new ApplicationRole[]
            {
                ApplicationRole.Administrator,
                ApplicationRole.User,
            };

            foreach (var item in orderedAppClaims)
            {
                if (user.Claims.FirstOrDefault(x => x.Type == claimType)?.Value == item.ToString())
                {
                    return item;
                }
            }

            return null;
        }

        public static Guid? GetUserId(ClaimsPrincipal user)
        {
            return Guid.TryParse(user.Claims.FirstOrDefault(x => x.Type == "uid" || x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value, out Guid result)
                ? result
                : null;
        }

        public static string? GetUserName(ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        }

        public static string? GetUserFirstName(ClaimsPrincipal user)
        {
            return GetUserName(user)?.Split(' ')[0];
        }

        public static string? GetUserSurname(ClaimsPrincipal user)
        {
            return GetUserName(user)?.Split(' ')[1];
        }
    }
}
