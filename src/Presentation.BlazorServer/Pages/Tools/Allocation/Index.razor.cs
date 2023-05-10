using MediatR;
using Microsoft.AspNetCore.Components;
using SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Commands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModulePreferenceCommands;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using ModulePreferenceQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModulePreferenceQueries;
using UserQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Tools.Allocation
{
    public partial class Index
    {
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Allocation";

        private IEnumerable<UserModel> Users { get; set; } = null!;
        private string UsersSearchString { get; set; } = string.Empty;

        private IEnumerable<ModulePreferenceDetailModel> ModulePreferences { get; set; } = null!;
        private bool ShowPendingResponseOnly { get; set; } = true;
        private string ModulePreferencesSearchString { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Users = (await Mediator.Send(new UserQueries.GetAllWithQuestionnaireToken.Query())).Resource;
            ModulePreferences = (await Mediator.Send(new ModulePreferenceQueries.GetAll.Query())).Resource;
        }

        private bool FilterUsersFunction(UserModel user) => FilterUsersFunction(user, UsersSearchString);
        private bool FilterUsersFunction(UserModel user, string searchString)
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

        private bool FilterModulePreferencesFunction(ModulePreferenceDetailModel modulePreference) => FilterModulePreferencesFunction(modulePreference, ModulePreferencesSearchString, ShowPendingResponseOnly);
        private bool FilterModulePreferencesFunction(ModulePreferenceDetailModel modulePreference, string searchString, bool showPendingResponseOnly)
        {
            if (ShowPendingResponseOnly)
            {
                var isPendingResponse = modulePreference.Status == Status.PendingResponse.ToString();

                if (string.IsNullOrWhiteSpace(searchString) && isPendingResponse)
                    return true;

                else if (modulePreference.Module.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) && isPendingResponse)
                    return true;
                else if (modulePreference.Module.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase) && isPendingResponse)
                    return true;
                else if (modulePreference.Module.Level.Contains(searchString, StringComparison.OrdinalIgnoreCase) && isPendingResponse)
                    return true;

                else if (modulePreference.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase) && isPendingResponse)
                    return true;
                else if (modulePreference.User.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase) && isPendingResponse)
                    return true;
                else if (modulePreference.User.AchievedLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase) && isPendingResponse)
                    return true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return true;

                else if (modulePreference.Module.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (modulePreference.Module.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (modulePreference.Module.Level.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;

                else if (modulePreference.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (modulePreference.User.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (modulePreference.User.AchievedLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private async Task AcceptModulePreferenceAsync(Guid userId, Guid moduleId)
        {
            try
            {
                var response = await Mediator.Send(new Update.Command(userId: userId,
                                                                      moduleId: moduleId,
                                                                      status: Status.Accepted.ToString()));

                var modulePreference = ModulePreferences.FirstOrDefault(x => x.User.Id == userId && x.Module.Id == moduleId);
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
        private async Task DeclineModulePreferenceAsync(Guid userId, Guid moduleId)
        {
            try
            {
                var response = await Mediator.Send(new Update.Command(userId: userId,
                                                                      moduleId: moduleId,
                                                                      status: Status.Declined.ToString()));

                var modulePreference = ModulePreferences.FirstOrDefault(x => x.User.Id == userId && x.Module.Id == moduleId);
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

        private async Task RunAllocationsAsync()
        {
            try
            {
                _ = await Mediator.Send(new Allocate.Command(algorithm: "FirstMatch"));

                NavigationManager.NavigateTo(uri: "/Tools/Allocation/Result");
            }
            catch
            {
                NavigationManager.NavigateTo(uri: "/500");
            }
        }
    }
}
