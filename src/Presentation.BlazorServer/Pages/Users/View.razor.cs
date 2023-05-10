using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.TimeAvailabilityModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using LabQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using UserQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Shared.Components;
using ModulePreferenceCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using TimeAvailabilityCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;
using UserModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Users
{
    public partial class View
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "User detail";

        private UserDetailModel? UserModel { get; set; }

        private static IReadOnlyList<TimeOnly> Times { get; } = new[]
        {
            new TimeOnly(09, 00),
            new TimeOnly(10, 00),
            new TimeOnly(11, 00),
            new TimeOnly(12, 00),
            new TimeOnly(13, 00),
            new TimeOnly(14, 00),
            new TimeOnly(15, 00),
            new TimeOnly(16, 00),
            new TimeOnly(17, 00),
        };

        private IReadOnlyList<TimeAvailabilityPageModel> TimeAvailabilities { get; set; } = null!;
        private static IReadOnlyList<TimeAvailabilityPageModel> GetTimeAvailabilityModels()
        {
            var timeAvailabilityInputModels = new List<TimeAvailabilityPageModel>();

            foreach (WorkDayOfWeek day in Enum.GetValues(typeof(WorkDayOfWeek)))
            {
                for (int i = Times.Min().Hour; i <= Times.Max().Hour; i++)
                {
                    timeAvailabilityInputModels.Add(new TimeAvailabilityPageModel()
                    {
                        Day = day,
                        StartTime = new TimeOnly(i, 0),
                        EndTime = new TimeOnly(i + 1, 0),
                        IsAvailable = false,
                        IsAllocated = false,
                        IsManuallyAllocated = false,
                    });
                }
            }

            return timeAvailabilityInputModels.AsReadOnly();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await LoadData();
        }

        private async Task LoadData()
        {
            TimeAvailabilities = GetTimeAvailabilityModels();
            UserModel = (await Mediator.Send(new UserQueries.GetDetail.Query(userId: UserId))).Resource;

            if (UserModel is not null)
            {
                foreach (var item in UserModel.TimeAvailabilities)
                {
                    var timeAvailability = TimeAvailabilities.FirstOrDefault(x =>
                        x.Day == item.Day &&
                        x.StartTime == item.StartTime);

                    if (timeAvailability is not null)
                    {
                        timeAvailability.IsAvailable = true;
                        timeAvailability.IsAllocated = item.IsAllocated;
                    }
                }

                await LoadManualAllocations();
            }
            else
            {
                NavigationManager.NavigateTo(uri: "/404");
            }
        }
        private async Task LoadManualAllocations()
        {
            foreach (var item in TimeAvailabilities)
            {
                item.IsManuallyAllocated = false;
            }

            var manuallyAllocatedLabs = (await Mediator.Send(new LabQueries.GetAllWhereUser.Query(userId: UserId))).Resource;

            foreach (var lab in manuallyAllocatedLabs)
            {
                var labStartHour = lab.StartTime.Hour;
                var labEndHour = lab.EndTime.Minute == 0 ? lab.EndTime.Hour : lab.EndTime.Hour + 1;

                var timeAvailabilities = TimeAvailabilities
                    .Where(x => x.Day == lab.Day
                             && x.StartTime.Hour >= labStartHour
                             && x.EndTime.Hour <= labEndHour
                             && x.IsAllocated == false);

                foreach (var item in timeAvailabilities)
                {
                    item.IsManuallyAllocated = true;
                }
            }
        }

        private async Task OpenDeleteUserConfirmationDialog(Guid userId)
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

                    if (response.Resource is null)
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

                    NavigationManager.NavigateTo(uri: "/Users");
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }

                StateHasChanged(); // TODO: Check if this code is needed
            }
        }
        private async Task OpenRemoveUserModuleRoleConfirmationDialog(Guid moduleId)
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
                .Show<DeleteConfirmationDialog>(title: "Remove Module Role",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    var response = await Mediator.Send(new UserModuleCommands.Delete.Command(userId: UserId, moduleId: moduleId));

                    if (response.Resource is not null)
                    {
                        await LoadData();
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
        private async Task OpenQuestionnaireResetConfirmationDialog(Guid userId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to reset time availability and module preference for this user?" },
                { "ConfirmButtonText", "Reset" },
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
                .Show<DeleteConfirmationDialog>(title: "Reset Questionnaire",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    _ = await Mediator.Send(new ModulePreferenceCommands.DeleteRange.Command(userId: userId));
                    _ = await Mediator.Send(new TimeAvailabilityCommands.DeleteRange.Command(userId: userId));

                    TimeAvailabilities = GetTimeAvailabilityModels();
                    UserModel!.TimeAvailabilities = Array.Empty<TimeAvailabilityModel>();
                    UserModel!.ModulePreferences = Array.Empty<UserDetailModulePreferenceModel>();

                    foreach (var item in TimeAvailabilities)
                    {
                        item.IsAvailable = false;
                    }

                    await LoadManualAllocations();

                    StateHasChanged();
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }

        private async Task OpenGenerateNewQuestionnaireTokenConfirmationDialog(Guid userId)
        {
            var parameters = new DialogParameters
            {
                { "ContentText", "Are you sure you want to allow this user to submit a new time availability and module preferences response? This action will delete existing response!" },
                { "ConfirmButtonText", "Confirm" },
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
                .Show<DeleteConfirmationDialog>(title: "Request a questionnaire response",
                                                parameters: parameters,
                                                options: options)
                .Result;

            if (result.Canceled == false)
            {
                try
                {
                    _ = await Mediator.Send(new ModulePreferenceCommands.DeleteRange.Command(userId: userId));
                    _ = await Mediator.Send(new TimeAvailabilityCommands.DeleteRange.Command(userId: userId));

                    var response = await Mediator.Send(new GenerateQuestionnaireToken.Command(userId: userId));

                    TimeAvailabilities = GetTimeAvailabilityModels();
                    UserModel!.TimeAvailabilities = Array.Empty<TimeAvailabilityModel>();
                    UserModel!.ModulePreferences = Array.Empty<UserDetailModulePreferenceModel>();

                    UserModel!.QuestionnaireToken = response.Resource.QuestionnaireToken;

                    await LoadManualAllocations();

                    StateHasChanged();
                }
                catch (EntityNotFoundException)
                {
                    NavigationManager.NavigateTo(uri: "/404");
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }

        private sealed class TimeAvailabilityPageModel
        {
            public WorkDayOfWeek Day { get; set; }
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
            public bool IsAvailable { get; set; }
            public bool IsAllocated { get; set; }
            public bool IsManuallyAllocated { get; set; }
        }
    }
}
