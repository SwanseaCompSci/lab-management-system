using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;
using LabCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;
using LabQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using LabScheduleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using UserLabCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserLabCommands;
using UserQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Labs
{
    public partial class View
    {
        [Parameter] public Guid LabId { get; set; }

        [Inject] public IDateTimeService DateTimeService { get; set; } = null!;
        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Lab detail";

        private LabDetailModel Model { get; set; } = null!;
        private bool IncludePastSchedules { get; set; } = false;

        private string UsersSearchString { get; set; } = null!;

        private MudForm AddStaffDialogForm { get; set; } = null!;
        private DialogOptions AddStaffDialogOptions { get; } = new()
        {
            Position = DialogPosition.Center,
            CloseOnEscapeKey = false,
            DisableBackdropClick = true,
            CloseButton = false,
        };
        private bool IsAddStaffDialogVisible { get; set; } = false;
        private UserModel? AddStaffDialogSelectedUser { get; set; }

        private async Task<IEnumerable<UserModel>> SearchUserAsync(string value)
        {
            return string.IsNullOrEmpty(value)
                ? Array.Empty<UserModel>()
                : (await Mediator.Send(new UserQueries.SearchInModuleButNotInLab.Query(moduleId: Model.ModuleId,
                                                                                       labId: LabId,
                                                                                       searchExpression: value))).Resource;
        }
        private void ShowAddStaffDialog()
        {
            AddStaffDialogSelectedUser = null;
            IsAddStaffDialogVisible = true;
        }
        private async Task AddStaffDialogConfirm()
        {
            await AddStaffDialogForm.Validate();

            if (AddStaffDialogForm.IsValid)
            {
                try
                {
                    var userLab = (await Mediator.Send(new UserLabCommands.Create.Command(userId: AddStaffDialogSelectedUser!.Id,
                                                                                          labId: LabId))).Resource;

                    if (userLab is not null)
                    {
                        var user = (await Mediator.Send(new UserQueries.Get.Query(userId: userLab.UserId))).Resource;

                        Model.Users = new List<UserModel>(Model.Users)
                        {
                            user!,
                        };

                        IsAddStaffDialogVisible = false;
                    }
                }
                catch (DuplicateEntityException)
                {
                    var errorDialogParameters = new DialogParameters
                    {
                        { "ContentText", $"User is already assigned to this lab." },
                        { "CloseButtonText", "Close" }
                    };

                    var errorDialogOptions = new DialogOptions()
                    {
                        Position = DialogPosition.Center,
                        CloseOnEscapeKey = false,
                        DisableBackdropClick = true,
                        CloseButton = false,
                    };

                    await DialogService.Show<ErrorDialog>(title: "Unable to add user",
                                                          parameters: errorDialogParameters,
                                                          options: errorDialogOptions).Result;
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var lab = (await Mediator.Send(new LabQueries.GetDetail.Query(labId: LabId))).Resource;
            if (lab is not null)
            {
                Model = lab;
            }
            else
            {
                NavigationManager.NavigateTo(uri: "/404");
            }
        }

        private bool FilterLabSchedulesFunction(LabScheduleModel labSchedule) => FilterLabSchedulesFunction(labSchedule, IncludePastSchedules);
        private bool FilterLabSchedulesFunction(LabScheduleModel labSchedule, bool includePastSchedules)
        {
            return includePastSchedules || labSchedule.Start >= DateTimeService.UtcNow;
        }

        private bool FilterUsersFunction(UserModel user) => FilterUsersFunction(user, UsersSearchString);
        private static bool FilterUsersFunction(UserModel user, string searchString)
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
                    _ = await Mediator.Send(new LabCommands.Delete.Command(labId: labId));
                    NavigationManager.NavigateTo(uri: $"/Modules/{Model.ModuleId}");
                }
                catch (Exception)
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
        private async Task OpenDeleteLabScheduleConfirmationDialog(Guid labScheduleId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to delete this lab schedule?" },
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
                .Show<DeleteConfirmationDialog>(title: "Delete Lab Schedule",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var response = await Mediator.Send(new LabScheduleCommands.Delete.Command(labScheduleId: labScheduleId));

                    if (response.Resource is not null)
                    {
                        var labSchedule = Model.LabSchedules.FirstOrDefault(x => x.Id.Equals(response.Resource.Id));
                        var labSchedules = new List<LabScheduleModel>(Model.LabSchedules);
                        labSchedules.Remove(labSchedule!);
                        Model.LabSchedules = labSchedules;
                    }
                    else
                    {
                        var errorDialogParameters = new DialogParameters
                        {
                            { "ContentText", $"Lab schedule does not exist." },
                            { "CloseButtonText", "Close" }
                        };

                        var errorDialogOptions = new DialogOptions()
                        {
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = false,
                            DisableBackdropClick = true,
                            CloseButton = false,
                        };

                        await DialogService.Show<ErrorDialog>(title: "Unable to delete lab schedule",
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

        private async Task OpenRemoveStaffMemberConfirmationDialog(Guid userId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to remove the member of staff from this lab?" },
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
                .Show<DeleteConfirmationDialog>(title: "Remove Member of Staff",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var commandResult = await Mediator.Send(new UserLabCommands.Delete.Command(userId: userId,
                                                                                               labId: LabId));
                    if (commandResult.Resource is not null)
                    {
                        var member = Model.Users.FirstOrDefault(x => x.Id.Equals(commandResult.Resource.UserId));
                        var membersOfStaff = new List<UserModel>(Model.Users);

                        membersOfStaff.Remove(member!);

                        Model.Users = membersOfStaff;
                    }
                    else
                    {
                        var errorDialogParameters = new DialogParameters
                        {
                            { "ContentText", $"User is not assigned to this lab." },
                            { "CloseButtonText", "Close" }
                        };

                        var errorDialogOptions = new DialogOptions()
                        {
                            Position = DialogPosition.Center,
                            CloseOnEscapeKey = false,
                            DisableBackdropClick = true,
                            CloseButton = false,
                        };

                        await DialogService.Show<ErrorDialog>(title: "Unable to remove user from lab",
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
