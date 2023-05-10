using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;
using System.ComponentModel.DataAnnotations;
using ModuleQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using UserModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Modules.Users
{
    public partial class Add
    {
        [Parameter] public Guid ModuleId { get; set; }

        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Add user to module";

        private ModuleModel? Module { get; set; } = null!;
        private Choice UserChoice { get; set; } = new();

        private MudForm Form { get; set; } = null!;
        private UserModuleCommands.Create.Command Command { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Module = (await Mediator.Send(new ModuleQueries.Get.Query(moduleId: ModuleId))).Resource;

            if (Module is not null)
            {
                Command = new UserModuleCommands.Create.Command()
                {
                    ModuleId = ModuleId,
                };
            }
            else
            {
                NavigationManager.NavigateTo(uri: $"/404");
            }
        }

        private async Task<IEnumerable<UserModel>> SearchUserAsync(string value)
        {
            return string.IsNullOrEmpty(value)
                ? Array.Empty<UserModel>()
                : (await Mediator.Send(new Search.Query(searchExpression: value))).Resource;
        }

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new UserModuleCommands.Create.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<UserModuleCommands.Create.Command>.CreateWithOptions((UserModuleCommands.Create.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task CreateUserModule()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                Command.UserId = UserChoice.User!.Id;

                try
                {
                    var result = await Mediator.Send(Command);

                    NavigationManager.NavigateTo(uri: $"/Modules/{result.Resource.ModuleId}");
                }
                catch (DuplicateEntityException)
                {
                    var errorDialogParameters = new DialogParameters
                    {
                        { "ContentText", $"User already has an existing role for the module." },
                        { "CloseButtonText", "Close" }
                    };

                    var errorDialogOptions = new DialogOptions()
                    {
                        Position = DialogPosition.Center,
                        CloseOnEscapeKey = false,
                        DisableBackdropClick = true,
                        CloseButton = false,
                    };

                    await DialogService.Show<ErrorDialog>(title: "Unable to assigne role to user",
                                                          parameters: errorDialogParameters,
                                                          options: errorDialogOptions).Result;
                }
                catch
                {
                    NavigationManager.NavigateTo("/500");
                }
            }
        }

        private class Choice
        {
            [Required]
            public UserModel? User { get; set; }
        }
    }
}
