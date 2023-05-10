using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Infrastructure.Shared.Services;

namespace SwanseaCompSci.LabManagementSystem.Infrastructure.Shared
{
    /// <summary>
    /// Provides methods for dependency injection of the shared infrastructure layer.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Adds all services required by the shared infrastructure layer.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> where to inject the services.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> with application configuration properties.</param>
        /// <returns><see cref="IServiceCollection"/> with the injected services.</returns>
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IDateTimeService, DateTimeService>();

            var azureAdSection = configuration.GetSection("AzureAd");
            var tenantId = azureAdSection.GetValue<string>("TenantId");
            var clientId = azureAdSection.GetValue<string>("ClientId");
            var clientSecret = azureAdSection.GetValue<string>("ClientSecret");

            services.AddScoped<IIdentityProvider, AADIdentityProvider>(x => new AADIdentityProvider(tenantId: tenantId,
                                                                                                    clientId: clientId,
                                                                                                    clientSecret: clientSecret));

            return services;
        }
    }
}
