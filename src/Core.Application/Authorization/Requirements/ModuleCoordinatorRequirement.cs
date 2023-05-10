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
    // TODO: Add code comments

    public class ModuleCoordinatorRequirement : IAuthorizationRequirement { }

    public class ModuleCoordinatorModuleRequirementHandler : AuthorizationHandler<ModuleCoordinatorRequirement, Module>
    {
        public ModuleCoordinatorModuleRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new(dbContext);
        }

        private ModuleCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ModuleCoordinatorRequirement requirement,
                                                       Module resource)
        {
            var userId = Helpers.GetUserId(context.User);
            var moduleId = resource.Id;

            if (Helper.IsModuleCoordinator(user: context.User,
                                           moduleId: resource.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }
    public class ModuleCoordinatorModuleModelRequirementHandler : AuthorizationHandler<ModuleCoordinatorRequirement, ModuleModel>
    {
        public ModuleCoordinatorModuleModelRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new(dbContext);
        }

        private ModuleCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ModuleCoordinatorRequirement requirement,
                                                       ModuleModel resource)
        {
            if (Helper.IsModuleCoordinator(user: context.User,
                                           moduleId: resource.Id))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.Id;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }

    public class ModuleCoordinatorLabRequirementHandler : AuthorizationHandler<ModuleCoordinatorRequirement, Lab>
    {
        public ModuleCoordinatorLabRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new(dbContext);
        }

        private ModuleCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ModuleCoordinatorRequirement requirement,
                                                       Lab resource)
        {
            if (Helper.IsModuleCoordinator(user: context.User,
                                           moduleId: resource.ModuleId))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.ModuleId;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }
    public class ModuleCoordinatorLabModelRequirementHandler : AuthorizationHandler<ModuleCoordinatorRequirement, LabModel>
    {
        public ModuleCoordinatorLabModelRequirementHandler(IApplicationDbContext dbContext)
        {
            Helper = new(dbContext);
        }

        private ModuleCoordinatorRequirementHandlerHelper Helper { get; }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       ModuleCoordinatorRequirement requirement,
                                                       LabModel resource)
        {
            if (Helper.IsModuleCoordinator(user: context.User,
                                           moduleId: resource.ModuleId))
            {
                context.Succeed(requirement);
            }
            else
            {
                var userId = Helpers.GetUserId(context.User);
                var moduleId = resource.ModuleId;

                context.Fail(new AuthorizationFailureReason(handler: this,
                                                            message: $"User ({userId}) does not have a {ModuleRole.ModuleCoordinator} role for Module ({moduleId})."));
            }

            return Task.CompletedTask;
        }
    }

    internal sealed class ModuleCoordinatorRequirementHandlerHelper
    {
        public ModuleCoordinatorRequirementHandlerHelper(IApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private IApplicationDbContext DbContext { get; }

        internal bool IsModuleCoordinator(ClaimsPrincipal user, Guid moduleId)
        {
            var userRole = Helpers.GetApplicationRole(user);

            // Administrators are allowed to do anything
            if (userRole == ApplicationRole.Administrator)
            {
                return true;
            }

            var userId = Helpers.GetUserId(user);
            var userModule = DbContext.UserModules.FirstOrDefault(x => x.UserId.Equals(userId) && x.ModuleId.Equals(moduleId));

            return userModule is not null && userModule.Role.Equals(ModuleRole.ModuleCoordinator);
        }
    }
}
