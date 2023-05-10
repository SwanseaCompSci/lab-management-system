using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using LabCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Labs
{
    public partial class Edit
    {
        [Parameter] public Guid LabId { get; set; }

        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Edit a lab";

        private MudForm Form { get; set; } = null!;
        private LabCommands.Update.Command Command { get; set; } = null!;
        private LabModel? Resource { get; set; } = null!;

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new LabCommands.Update.CommandValidator();

            var result = await validator.ValidateAsync(ValidationContext<LabCommands.Update.Command>.CreateWithOptions((LabCommands.Update.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Resource = (await Mediator.Send(new Get.Query(labId: LabId))).Resource;
            if (Resource is not null)
            {
                Command = new LabCommands.Update.Command
                {
                    Id = LabId,
                    ModuleId = Resource.ModuleId,
                    Name = Resource.Name,
                    Day = Resource.Day.ToString(),
                    StartTime = Resource.StartTime.ToTimeSpan(),
                    EndTime = Resource.EndTime.ToTimeSpan(),
                    MinNumberOfStaff = Resource.MinNumberOfStaff,
                    MaxNumberOfStaff = Resource.MaxNumberOfStaff,
                };
            }
            else
            {
                NavigationManager.NavigateTo("/404");
            }
        }

        private async Task UpdateLab()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Command);

                    NavigationManager.NavigateTo(uri: $"/Labs/{result.Resource.Id}");
                }
                catch (EntityNotFoundException)
                {
                    NavigationManager.NavigateTo("/404");
                }
                catch (Exception)
                {
                    NavigationManager.NavigateTo("/500");
                }
            }
        }
    }
}
