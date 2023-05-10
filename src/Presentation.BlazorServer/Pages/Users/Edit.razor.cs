using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using UserCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.UserCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Users
{
    public partial class Edit
    {
        [Parameter] public Guid UserId { get; set; }

        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Edit a user";

        private MudForm Form { get; set; } = null!;
        private UserCommands.Update.Command Command { get; set; } = null!;
        private UserModel? Resource { get; set; } = null!;

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new UserCommands.Update.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<UserCommands.Update.Command>.CreateWithOptions((UserCommands.Update.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Resource = (await Mediator.Send(new Get.Query(userId: UserId))).Resource;
            if (Resource is not null)
            {
                Command = new UserCommands.Update.Command
                {
                    Id = Resource.Id,
                    FirstName = Resource.FirstName,
                    Surname = Resource.Surname,
                    AchievedLevel = Resource.AchievedLevel,
                    MaxWeeklyWorkHours = Resource.MaxWeeklyWorkHours,
                };
            }
            else
            {
                NavigationManager.NavigateTo(uri: $"/404");
            }
        }

        private async Task UpdateUser()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Command);

                    NavigationManager.NavigateTo(uri: $"/Users/{result.Resource.Id}");
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
