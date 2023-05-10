using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components
{
    public partial class ErrorDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public string ContentText { get; set; } = string.Empty;
        [Parameter] public string CloseButtonText { get; set; } = string.Empty;

        void Close() => MudDialog.Close();
    }
}
