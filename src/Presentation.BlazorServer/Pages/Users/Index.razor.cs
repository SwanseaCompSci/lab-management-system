using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Users
{
    public partial class Index
    {
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Users";

        private IEnumerable<UserModel> Users { get; set; } = null!;
        private string SearchString { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Users = (await Mediator.Send(new GetAll.Query()))
                .Resource
                .OrderBy(x => x.Surname)
                .ThenBy(x => x.FirstName);
        }

        private bool FilterFunction(UserModel user) => FilterFunction(user, SearchString);
        private static bool FilterFunction(UserModel user, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            else if (user.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (user.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (user.AchievedLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task OpenDeleteConfirmationDialog(Guid userId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete this user?" },
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
                .Show<DeleteConfirmationDialog>(title: "Delete User",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var response = await Mediator.Send(new Delete.Command(userId: userId));

                    if (response.Resource is not null)
                    {
                        var users = new List<UserModel>(Users);
                        var user = users.FirstOrDefault(x => x.Id == response.Resource.Id);
                        users.Remove(user!);

                        Users = users;
                    }
                    else
                    {
                        var errorDialogParameters = new DialogParameters
                        {
                            { "ContentText", $"User does not exist." },
                            { "CloseButtonText", "Close" }
                        };

                        var errorDialogOptions = new DialogOptions()
                        {
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = false,
                            DisableBackdropClick = true,
                            CloseButton = false,
                        };

                        await DialogService.Show<ErrorDialog>(title: "Unable to delete user",
                                                              parameters: errorDialogParameters,
                                                              options: errorDialogOptions).Result;
                    }
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }

                StateHasChanged();
            }
        }
    }
}
