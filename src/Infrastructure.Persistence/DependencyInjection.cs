using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Persistence.Repositories.Common;
using System.Reflection;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Persistence
{
    /// <summary>
    /// Provides methods for dependency injection of the persistence infrastructure layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all services required by the persistence infrastructure layer.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> where to inject the services.</param>
        /// <returns><see cref="IServiceCollection"/> with the injected services.</returns>
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                optionsAction: options => options.UseSqlServer(
                    configuration.GetConnectionString("DbConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)),
                contextLifetime: ServiceLifetime.Transient);

            services.AddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddRepositories(Assembly.GetExecutingAssembly());

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            // It works, don't touch it

            var repositories = assembly.GetTypes()
                .Where(item =>
                    item.GetInterfaces().Where(i => i.IsGenericType).Any(i => i.GetGenericTypeDefinition() == typeof(IRepository<>))
                    && !item.IsAbstract
                    && !item.IsInterface);

            foreach (var item in repositories)
            {
                // Get only interfaces that are not generic (e.g. IModuleRepository) and implement generic interface (e.g. IRepository<T>)
                var serviceType = item
                    .GetInterfaces()
                    .FirstOrDefault(x =>
                        !x.IsGenericType &&
                        x.GetInterfaces().Any(i => i.GetGenericTypeDefinition().Equals(typeof(IRepository<>))));

                if (serviceType is not null)
                {
                    services.AddScoped(serviceType, item);
                }
            }

            return services;
        }
    }
}
