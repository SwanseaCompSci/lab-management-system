using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using ModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Modules
{
    public partial class Edit
    {
        [Parameter] public Guid ModuleId { get; set; }

        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Edit a module";

        private MudForm Form { get; set; } = null!;
        private ModuleCommands.Update.Command Command { get; set; } = null!;
        private ModuleModel? Resource { get; set; } = null!;

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new ModuleCommands.Update.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<ModuleCommands.Update.Command>.CreateWithOptions((ModuleCommands.Update.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Resource = (await Mediator.Send(new Get.Query(moduleId: ModuleId))).Resource;
            if (Resource is not null)
            {
                Command = new ModuleCommands.Update.Command
                {
                    Id = Resource.Id,
                    Name = Resource.Name,
                    Code = Resource.Code,
                    Level = Resource.Level
                };
            }
            else
            {
                NavigationManager.NavigateTo("/404");
            }
        }

        private async Task UpdateModule()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Command);

                    NavigationManager.NavigateTo(uri: $"/Modules/{result.Resource.Id}");
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
