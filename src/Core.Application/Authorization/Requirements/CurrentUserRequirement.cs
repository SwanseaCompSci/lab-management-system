using Microsoft.AspNetCore.Authorization;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Authorization.Requirements
{
    public class CurrentUserRequirement : IAuthorizationRequirement { }

    public class CurrentUserRequirementHandler : AuthorizationHandler<CurrentUserRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CurrentUserRequirement requirement, User resource)
        {
            if (CurrentUserRequirementHandlerHelper.IsCurrentUser(user: context.User,
                                                                  userId: resource.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                var currentUserId = Helpers.GetUserId(user: context.User);

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({currentUserId}) is not allowed to access information about User ({resource.Id})."));
            }

            return Task.CompletedTask;
        }
    }

    public class CurrentUserModelRequirementHandler : AuthorizationHandler<CurrentUserRequirement, UserModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CurrentUserRequirement requirement, UserModel resource)
        {
            if (CurrentUserRequirementHandlerHelper.IsCurrentUser(user: context.User,
                                                                  userId: resource.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                var currentUserId = Helpers.GetUserId(user: context.User);

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({currentUserId}) is not allowed to access information about User ({resource.Id})."));
            }

            return Task.CompletedTask;
        }
    }

    internal static class CurrentUserRequirementHandlerHelper
    {
        internal static bool IsCurrentUser(ClaimsPrincipal user, Guid userId)
        {
            return Helpers.GetApplicationRole(user) == ApplicationRole.Administrator || Helpers.GetUserId(user).Equals(userId);
        }
    }
}
