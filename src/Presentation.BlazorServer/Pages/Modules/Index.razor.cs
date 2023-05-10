using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Modules
{
    public partial class Index
    {
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Modules";

        private IEnumerable<ModuleModel> Modules { get; set; } = null!;
        private string SearchString { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Modules = (await Mediator.Send(new GetAll.Query())).Resource.OrderBy(x => x.Name);
        }

        private bool FilterFunction(ModuleModel module) => FilterFunction(module, SearchString);
        private static bool FilterFunction(ModuleModel module, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            else if (module.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (module.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (module.Level.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task OpenDeleteConfirmationDialog(Guid moduleId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete this module?" },
                { "ConfirmButtonText", "Delete" },
                { "CancelButtonText", "Cancel" }
            };

            var options = new DialogOptions()
            {
                Position = DialogPosition.Center,
                CloseOnEscapeKey = false,
                DisableBackdropClick = true,
                CloseButton = false,
            };

            var result = await DialogService
                .Show<DeleteConfirmationDialog>(title: "Delete Module",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var response = await Mediator.Send(new Delete.Command(moduleId: moduleId));

                    if (response.Resource is not null)
                    {
                        var modules = new List<ModuleModel>(Modules);
                        var module = modules.FirstOrDefault(x => x.Id == response.Resource.Id);
                        modules.Remove(module!);

                        Modules = modules.AsEnumerable();
                    }
                    else
                    {
                        var errorDialogParameters = new DialogParameters
                        {
                            { "ContentText", "Module does not exist." },
                            { "CloseButtonText", "Close" }
                        };

                        var errorDialogOptions = new DialogOptions()
                        {
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = false,
                            DisableBackdropClick = true,
                            CloseButton = false,
                        };

                        await DialogService.Show<ErrorDialog>(title: "Unable to delete module",
                                                              parameters: errorDialogParameters,
                                                              options: errorDialogOptions).Result;
                    }
                }
                catch (Exception)
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }

                StateHasChanged();
            }
        }
    }
}
