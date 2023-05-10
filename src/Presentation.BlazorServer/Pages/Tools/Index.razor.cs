using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;
using ModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using ModulePreferenceCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using TimeAvailabilityCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;
using UserCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Tools
{
    public partial class Index
    {
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Tools";

        public async Task SendQuestionnairesAsync()
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to send questionnaires to all users?" },
                { "ConfirmButtonText", "Yes" },
                { "CancelButtonText", "No" }
            };

            var options = new DialogOptions()
            {
                Position = DialogPosition.Center,
                CloseOnEscapeKey = false,
                DisableBackdropClick = true,
                CloseButton = false,
            };

            var result = await DialogService
                .Show<DeleteConfirmationDialog>(title: "Send Questionnaires",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                _ = await Mediator.Send(new ModulePreferenceCommands.DeleteAll.Command());
                _ = await Mediator.Send(new TimeAvailabilityCommands.DeleteAll.Command());
                _ = await Mediator.Send(new UserCommands.GenerateQuestionnaireTokenForAll.Command());
            }
        }

        public async Task ResetSystemAsync()
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to reset the system?" },
                { "ConfirmButtonText", "Yes" },
                { "CancelButtonText", "No" }
            };

            var options = new DialogOptions()
            {
                Position = DialogPosition.Center,
                CloseOnEscapeKey = false,
                DisableBackdropClick = true,
                CloseButton = false,
            };

            var result = await DialogService
                .Show<DeleteConfirmationDialog>(title: "System Reset",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                _ = await Mediator.Send(new ModuleCommands.DeleteAll.Command());
                _ = await Mediator.Send(new UserCommands.DeleteAll.Command());
            }
        }
    }
}
