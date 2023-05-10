using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using ModulePreferenceCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using TimeAvailabilityCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.TimeAvailabilityCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Questionnaire
{
    public partial class Index
    {
        [Parameter] public Guid Token { get; set; }

        [Inject] public IDialogService DialogService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Submit time availability";

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

        private MudTabs Tabs { get; set; } = null!;
        private MudTabPanel TimeAvailabilityTabPanel { get; set; } = null!;
        private MudTabPanel ModulePreferencesTabPanel { get; set; } = null!;

        private UserModel? UserModel { get; set; }
        private IReadOnlyList<TimeAvailabilityInputModel> TimeAvailabilities { get; set; } = null!;
        private static IReadOnlyList<TimeAvailabilityInputModel> GetTimeAvailabilityInputModels()
        {
            var timeAvailabilityInputModels = new List<TimeAvailabilityInputModel>();

            foreach (WorkDayOfWeek day in Enum.GetValues(typeof(WorkDayOfWeek)))
            {
                for (int i = Times.Min().Hour; i <= Times.Max().Hour; i++)
                {
                    timeAvailabilityInputModels.Add(new TimeAvailabilityInputModel()
                    {
                        WorkDayOfWeek = day,
                        StartTime = new TimeOnly(i, 0),
                        EndTime = new TimeOnly(i + 1, 0),
                        Selected = false,
                    });
                }
            }

            return timeAvailabilityInputModels.AsReadOnly();
        }
        private IReadOnlyList<ModulePreferenceInputModel> ModulePreferences { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            UserModel = (await Mediator.Send(new GetByQuestionnaireToken.Query(token: Token))).Resource;
            if (UserModel is not null)
            {
                TimeAvailabilities = GetTimeAvailabilityInputModels();

                ModulePreferences = (await Mediator.Send(new GetAllBelowOrEqualLevel.Query(level: UserModel.AchievedLevel)))
                    .Resource
                    .Select(x => new ModulePreferenceInputModel()
                    {
                        Module = x,
                        Selected = false,
                    }).ToList().AsReadOnly();
            }
            else
            {
                NavigationManager.NavigateTo(uri: "/404");
            }
        }

        private void ShowTimeAvailabilityTab()
        {
            Tabs.ActivatePanel(panel: TimeAvailabilityTabPanel);
        }
        private void ShowModulePreferencesTab()
        {
            Tabs.ActivatePanel(panel: ModulePreferencesTabPanel);
        }

        private async Task SubmitQuestionnaire()
        {
            var selectedTimeAvailabilities = TimeAvailabilities
                .Where(x => x.Selected)
                .Select(x => new TimeAvailabilityCommands.CreateRange.TimeAvailabilityCommandModel()
                {
                    Day = x.WorkDayOfWeek.ToString(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                });

            var timeCommand = new TimeAvailabilityCommands.CreateRange.Command()
            {
                UserId = UserModel!.Id,
                Token = Token,
                TimeAvailabilities = selectedTimeAvailabilities,
            };
            var preferencesCommand = new ModulePreferenceCommands.CreateRange.Command()
            {
                UserId = UserModel!.Id,
                ModuleIds = ModulePreferences.Where(x => x.Selected).Select(x => x.Module.Id),
            };

            _ = await Mediator.Send(timeCommand);
            _ = await Mediator.Send(preferencesCommand);

            NavigationManager.NavigateTo(uri: $"/Users/{UserModel!.Id}");
        }

        private sealed class ModulePreferenceInputModel
        {
            public ModuleModel Module { get; set; } = null!;
            public bool Selected { get; set; }
        }
        private sealed class TimeAvailabilityInputModel
        {
            public WorkDayOfWeek WorkDayOfWeek { get; set; }
            public TimeOnly StartTime { get; set; }
            public TimeOnly EndTime { get; set; }
            public bool Selected { get; set; }
        }
    }
}
