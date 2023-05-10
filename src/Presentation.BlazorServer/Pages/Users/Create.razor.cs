using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;
using UserCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Users
{
    public partial class Create
    {
        [Inject] public IIdentityProvider IdentityProvider { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Create a new user";

        private MudForm Form { get; set; } = null!;

        private UserModel? _user;
        public UserModel? SelectedUser
        {
            get { return _user; }
            set
            {
                _user = value;

                Model = value is not null
                    ? new UserCommands.Create.Command()
                    {
                        Id = value.Id,
                        FirstName = value.FirstName,
                        Surname = value.Surname,
                    }
                    : new UserCommands.Create.Command();
            }
        }

        internal UserCommands.Create.Command Model { get; set; } = new();

        private async Task<IEnumerable<UserModel>> SearchUserAsync(string searchExpression)
        {
            if (string.IsNullOrEmpty(searchExpression))
            {
                return Array.Empty<UserModel>();
            }

            var azureUserModels = (await IdentityProvider.SearchUsersAsync(searchExpression: searchExpression)).ToList();
            var appUserModels = (await Mediator.Send(new Search.Query(searchExpression: searchExpression))).Resource.ToList();

            foreach (var user in appUserModels)
            {
                azureUserModels.RemoveAll(x => x.Id == user.Id);
            }

            return azureUserModels;
        }

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new UserCommands.Create.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<UserCommands.Create.Command>.CreateWithOptions((UserCommands.Create.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task CreateUser()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Model);

                    NavigationManager.NavigateTo(uri: $"/Users/{result.Resource.Id}");
                }
                catch (DuplicateEntityException)
                {
                    // TODO: Show error dialog
                    var errorDialogParameters = new DialogParameters
                    {
                        { "ContentText", $"User profile already exists." },
                        { "CloseButtonText", "Close" }
                    };

                    var errorDialogOptions = new DialogOptions()
                    {
                        Position = DialogPosition.Center,
                        CloseOnEscapeKey = false,
                        DisableBackdropClick = true,
                        CloseButton = false,
                    };

                    await DialogService.Show<ErrorDialog>(title: "Unable to create user",
                                                          parameters: errorDialogParameters,
                                                          options: errorDialogOptions).Result;
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
    }
}
