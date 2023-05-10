using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.ModuleQueries;
using LabCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Labs
{
    public partial class Create
    {
        [Parameter] public Guid ModuleId { get; set; }

        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Create a new lab";

        private MudForm Form { get; set; } = null!;
        private LabCommands.Create.Command Model { get; set; } = new();
        private Get.Response ResourceResponse { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ResourceResponse = await Mediator.Send(new Get.Query(moduleId: ModuleId));
        }

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new LabCommands.Create.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<LabCommands.Create.Command>.CreateWithOptions((LabCommands.Create.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task CreateLab()
        {
            Model.ModuleId = ModuleId;

            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Model);

                    NavigationManager.NavigateTo(uri: $"/Labs/{result.Resource.Id}");
                }
                catch (Exception)
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
    }
}
