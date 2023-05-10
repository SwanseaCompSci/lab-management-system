using Microsoft.AspNetCore.Authorization;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using System.Security.Claims;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Authorization.Requirements
{
    public class LabCoordinatorRequirement : IAuthorizationRequirement { }

    public class LabCoordinatorLabRequirementHandler : AuthorizationHandler<LabCoordinatorRequirement, Lab>
    {
        public LabCoordinatorLabRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new LabCoordinatorRequirementHandlerHelper(dbContext);
        }

        private LabCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LabCoordinatorRequirement requirement, Lab resource)
        {
            if (Helper.IsLabCoordinator(user: context.User,
                                        moduleId: resource.ModuleId))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.ModuleId;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} or {ModuleRole.LabCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }
    public class LabCoordinatorLabModelRequirementHandler : AuthorizationHandler<LabCoordinatorRequirement, LabModel>
    {
        public LabCoordinatorLabModelRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new LabCoordinatorRequirementHandlerHelper(dbContext);
        }

        private LabCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LabCoordinatorRequirement requirement, LabModel resource)
        {
            if (Helper.IsLabCoordinator(user: context.User,
                                        moduleId: resource.ModuleId))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.ModuleId;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} or {ModuleRole.LabCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }

    public class LabCoordinatorModuleRequirementHandler : AuthorizationHandler<LabCoordinatorRequirement, Module>
    {
        public LabCoordinatorModuleRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new LabCoordinatorRequirementHandlerHelper(dbContext);
        }

        private LabCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LabCoordinatorRequirement requirement, Module resource)
        {
            if (Helper.IsLabCoordinator(user: context.User,
                                        moduleId: resource.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.Id;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} or {ModuleRole.LabCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }
    public class LabCoordinatorModuleModelRequirementHandler : AuthorizationHandler<LabCoordinatorRequirement, ModuleModel>
    {
        public LabCoordinatorModuleModelRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new LabCoordinatorRequirementHandlerHelper(dbContext);
        }

        private LabCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LabCoordinatorRequirement requirement, ModuleModel resource)
        {
            if (Helper.IsLabCoordinator(user: context.User,
                                        moduleId: resource.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.Id;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} or {ModuleRole.LabCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class LabCoordinatorRequirementHandlerHelper
    {
        public LabCoordinatorRequirementHandlerHelper(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private IApplicationDbContext DbContext { get; }

        internal bool IsLabCoordinator(ClaimsPrincipal user, Guid moduleId)
        {
            var userRole = Helpers.GetApplicationRole(user);

            // Administrators are allowed to do anything
            if (userRole == ApplicationRole.Administrator)
            {
                return true;
            }

            var userId = Helpers.GetUserId(user);

            var userModule = DbContext.UserModules.FirstOrDefault(x => x.UserId.Equals(userId) && x.ModuleId.Equals(moduleId));
            return userModule is not null && (userModule.Role == ModuleRole.ModuleCoordinator || userModule.Role == ModuleRole.LabCoordinator);
        }
    }
}
