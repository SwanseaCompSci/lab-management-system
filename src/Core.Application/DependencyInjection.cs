using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SwanseaCompSci.LabManagementSystem.Core.Application.Authorization;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Enums;
using System.Reflection;

namespace SwanseaCompSci.LabManagementSystem.Core.Application
{
    /// <summary>
    /// Provides methods for dependency injection of the application layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all services required by the application layer.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> where to inject the services.</param>
        /// <returns><see cref="IServiceCollection"/> with the injected services.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(executingAssembly));

            // Authorization
            services.AddAuthorizationCore(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireRole(new string[]
                    {
                        ApplicationRole.Administrator.ToString(),
                        ApplicationRole.User.ToString(),
                    })
                    .Build();

                options.AddPolicy(name: Policies.IsModuleCoordinator, policy: Policies.IsModuleCoordinatorPolicy());
                options.AddPolicy(name: Policies.IsLabCoordinator, policy: Policies.IsLabCoordinatorPolicy());
                options.AddPolicy(name: Policies.IsCurrentUser, policy: Policies.IsCurrentUserPolicy());

                // By default, all incoming requests will be authorized according to the default policy
                options.FallbackPolicy = options.DefaultPolicy;
            });
            services.AddRequirementHandlers(executingAssembly);

            return services;
        }

        // TODO: Add docs comments
        private static IServiceCollection AddRequirementHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlers = assembly.DefinedTypes.Where(x => typeof(IAuthorizationHandler).IsAssignableFrom(x) && !x.IsAbstract).Distinct();

            foreach (var item in handlers)
            {
                services.AddScoped(typeof(IAuthorizationHandler), item);
            }

            return services;
        }
    }
}
