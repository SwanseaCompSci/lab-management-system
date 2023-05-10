using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation
{
    /// <summary>
    /// Provides methods for dependency injection of the application allocation layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all services required by the application allocation layer.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> where to inject the services.</param>
        /// <returns><see cref="IServiceCollection"/> with the injected services.</returns>
        public static IServiceCollection AddApplicationAllocationServices(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(executingAssembly);
            services.AddValidatorsFromAssembly(executingAssembly);
            services.AddMediatR(config => config.RegisterServicesFromAssembly(executingAssembly));

            return services;
        }
    }
}
