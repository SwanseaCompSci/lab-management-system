using Microsoft.AspNetCore.Components;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared
{
    public partial class MainLayout
    {
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;
        [Inject] public ICurrentUserService CurrentUserService { get; set; } = null!;
    }
}
