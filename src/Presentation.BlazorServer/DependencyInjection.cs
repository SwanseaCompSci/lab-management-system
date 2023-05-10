using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using MudBlazor.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Services;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer
{
    // TODO: Add docs comments
    internal static class DependencyInjection
    {
        internal static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddMudServices();

            // AAD Integration
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));
            services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();

            // Application
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
