using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;
using LabCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using ModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;
using UserModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Modules
{
    public partial class View
    {
        [Parameter] public Guid ModuleId { get; set; }

        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Module detail";

        private ModuleDetailModel? Model { get; set; }

        private string UsersSearchString { get; set; } = null!;
        private string ModulePreferenceSearchString { get; set; } = null!;

        private bool ModulePreferencesAlertClosed { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var resource = (await Mediator.Send(new GetDetail.Query(moduleId: ModuleId))).Resource;
            if (resource is null)
            {
                NavigationManager.NavigateTo("/404");
            }
            else
            {
                Model = resource;
            }
        }

        private bool FilterUsersFunction(ModuleDetailUserRoleModel userRole) => FilterUsersFunction(userRole, UsersSearchString);
        private static bool FilterUsersFunction(ModuleDetailUserRoleModel userRole, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            else if (userRole.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (userRole.User.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (userRole.User.AchievedLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (userRole.Role.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private bool FilterModulePreferencesFunction(ModulePreferenceDetailModel modulePreference) => FilterModulePreferencesFunction(modulePreference, ModulePreferenceSearchString);
        private static bool FilterModulePreferencesFunction(ModulePreferenceDetailModel modulePreference, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            else if (modulePreference.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (modulePreference.User.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (modulePreference.User.AchievedLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private async Task OpenDeleteModuleConfirmationDialog(Guid moduleId)
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
                    _ = await Mediator.Send(new ModuleCommands.Delete.Command(moduleId: moduleId));
                    NavigationManager.NavigateTo("/Modules");
                }
                catch (EntityNotFoundException)
                {
                    var errorDialogParameters = new DialogParameters
                    {
                        { "ContentText", $"Module does not exist." },
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
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private async Task OpenDeleteLabConfirmationDialog(Guid labId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete this lab?" },
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
                .Show<DeleteConfirmationDialog>(title: "Delete Lab",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var response = await Mediator.Send(new LabCommands.Delete.Command(labId: labId));

                    if (response.Resource is not null)
                    {
                        var lab = Model!.Labs.FirstOrDefault(x => x.Id.Equals(response.Resource.Id));
                        var labs = new List<LabModel>(Model.Labs);
                        labs.Remove(lab!);
                        Model.Labs = labs;
                    }
                    else
                    {
                        var errorDialogParameters = new DialogParameters
                        {
                            { "ContentText", $"Lab does not exist." },
                            { "CloseButtonText", "Close" }
                        };

                        var errorDialogOptions = new DialogOptions()
                        {
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = false,
                            DisableBackdropClick = true,
                            CloseButton = false,
                        };

                        await DialogService.Show<ErrorDialog>(title: "Unable to delete lab",
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
        private async Task OpenRemoveUserModuleRoleConfirmationDialog(Guid userId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to remove the user from this module?" },
                { "ConfirmButtonText", "Remove" },
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
                .Show<DeleteConfirmationDialog>(title: "Remove User",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var response = await Mediator.Send(new UserModuleCommands.Delete.Command(userId: userId, moduleId: ModuleId));

                    if (response.Resource is not null)
                    {
                        var userRole = Model!.UserRoles.FirstOrDefault(x => x.User.Id.Equals(response.Resource.UserId));
                        var userRoles = new List<ModuleDetailUserRoleModel>(Model.UserRoles);
                        userRoles.Remove(userRole!);
                        Model.UserRoles = userRoles;
                    }
                    else
                    {
                        var errorDialogParameters = new DialogParameters
                        {
                            { "ContentText", $"User is not assigned to this module." },
                            { "CloseButtonText", "Close" }
                        };

                        var errorDialogOptions = new DialogOptions()
                        {
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = false,
                            DisableBackdropClick = true,
                            CloseButton = false,
                        };

                        await DialogService.Show<ErrorDialog>(title: "Unable to remove user from this module",
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

        private async Task OpenAcceptModulePreferenceConfirmationDialog(Guid userId)
        {
            try
            {
                var response = await Mediator.Send(new Update.Command(userId: userId,
                                                                      moduleId: Model!.Id,
                                                                      status: Status.Accepted.ToString()));

                var modulePreference = Model.ModulePreferences.FirstOrDefault(x => x.User.Id == userId);
                if (modulePreference is not null)
                {
                    modulePreference.Status = response.Resource.Status;
                }
            }
            catch (EntityNotFoundException)
            {
                NavigationManager.NavigateTo(uri: "/404");
            }
            catch
            {
                NavigationManager.NavigateTo(uri: "/500");
            }

            StateHasChanged();
        }
        private async Task OpenDeclineModulePreferenceConfirmationDialog(Guid userId)
        {
            try
            {
                var response = await Mediator.Send(new Update.Command(userId: userId,
                                                                      moduleId: Model!.Id,
                                                                      status: Status.Declined.ToString()));

                var modulePreference = Model.ModulePreferences.FirstOrDefault(x => x.User.Id == userId);
                if (modulePreference is not null)
                {
                    modulePreference.Status = response.Resource.Status;
                }
            }
            catch (EntityNotFoundException)
            {
                NavigationManager.NavigateTo(uri: "/404");
            }
            catch
            {
                NavigationManager.NavigateTo(uri: "/500");
            }

            StateHasChanged();
        }
    }
}
