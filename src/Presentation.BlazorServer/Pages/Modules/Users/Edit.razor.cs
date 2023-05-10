using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using UserModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserModuleCommands;
using UserModuleQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserModuleQueries;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Modules.Users
{
    public partial class Edit
    {
        [Parameter] public Guid ModuleId { get; set; }
        [Parameter] public Guid UserId { get; set; }

        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Edit user permission for module";

        private UserModuleDetailModel? UserModuleDetail { get; set; } = null!;
        private string UserDisplayName { get; set; } = null!;

        private MudForm Form { get; set; } = null!;
        private UserModuleCommands.Update.Command Command { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            UserModuleDetail = (await Mediator.Send(new UserModuleQueries.GetDetail.Query(userId: UserId, moduleId: ModuleId))).Resource;

            if (UserModuleDetail is not null)
            {
                UserDisplayName = $"{UserModuleDetail.UserFirstName} {UserModuleDetail.UserSurname}";

                Command = new UserModuleCommands.Update.Command()
                {
                    ModuleId = UserModuleDetail.ModuleId,
                    UserId = UserModuleDetail.UserId,
                    Role = UserModuleDetail.Role,
                };
            }
            else
            {
                NavigationManager.NavigateTo(uri: $"/404");
            }
        }

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new UserModuleCommands.Update.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<UserModuleCommands.Update.Command>.CreateWithOptions((UserModuleCommands.Update.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task UpdateUserModuleAsync()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Command);

                    NavigationManager.NavigateTo(uri: $"/Modules/{result.Resource.ModuleId}");
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
    }
}
