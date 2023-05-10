using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using ModuleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.ModuleCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Modules
{
    public partial class Create
    {
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Create a new module";

        private MudForm Form { get; set; } = null!;
        private ModuleCommands.Create.Command Model { get; set; } = new();

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new ModuleCommands.Create.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<ModuleCommands.Create.Command>.CreateWithOptions((ModuleCommands.Create.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task CreateModule()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Model);

                    NavigationManager.NavigateTo(uri: $"/Modules/{result.Resource.Id}");
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
    }
}
