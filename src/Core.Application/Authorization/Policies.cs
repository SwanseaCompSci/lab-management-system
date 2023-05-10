using Microsoft.AspNetCore.Authorization;
using SwanseaCompSci.LabManagementSystem.Core.Application.Authorization.Requirements;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Authorization
{
    public static class Policies
    {
        public const string IsModuleCoordinator = "IsModuleCoordinator";
        public const string IsLabCoordinator = "IsLabCoordinator";
        public const string IsCurrentUser = "IsCurrentUser";

        public static AuthorizationPolicy IsModuleCoordinatorPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new ModuleCoordinatorRequirement())
                .Build();
        }
        public static AuthorizationPolicy IsLabCoordinatorPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new LabCoordinatorRequirement())
                .Build();
        }
        public static AuthorizationPolicy IsCurrentUserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new CurrentUserRequirement())
                .Build();
        }
    }
}
