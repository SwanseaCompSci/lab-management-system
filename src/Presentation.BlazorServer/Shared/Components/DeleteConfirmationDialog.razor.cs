using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components
{
    public partial class DeleteConfirmationDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public string ContentText { get; set; } = string.Empty;
        [Parameter] public string ConfirmButtonText { get; set; } = string.Empty;
        [Parameter] public string CancelButtonText { get; set; } = string.Empty;

        void Confirm() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();
    }
}
