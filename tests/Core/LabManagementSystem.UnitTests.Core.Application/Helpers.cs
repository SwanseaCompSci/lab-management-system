using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Common;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.UnitTests.Core.Application
{
    internal static class Helpers
    {
        public static void SetPrivatePropertyValue<T>(this BaseEntity entity, string propName, T newValue)
        {
            var propertyInfo = entity.GetType().GetProperty(propName);
            if (propertyInfo is not null)
            {
                propertyInfo.SetValue(entity, newValue);
            }
        }

        public static ClaimsPrincipal GetClaimsPrincipal(User user, bool isAdmin)
        {
            var claims = new List<Claim>()
            {
                new Claim(type: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                          value: isAdmin ? ApplicationRole.Administrator.ToString() : ApplicationRole.User.ToString()),
                new Claim(type: "http://schemas.microsoft.com/identity/claims/objectidentifier",
                          value: user.Id.ToString()),
                new Claim(type: "name",
                          value: $"{user.FirstName} {user.Surname}"),
            };
            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }
    }
}
