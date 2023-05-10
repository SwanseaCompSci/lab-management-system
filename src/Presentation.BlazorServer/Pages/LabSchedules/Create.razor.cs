using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using LabQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using LabScheduleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.LabSchedules
{
    public partial class Create
    {
        [Parameter] public Guid LabId { get; set; }

        [Inject] public IDateTimeService DateTimeService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Create a new lab schedule";

        private MudForm SingleLabScheduleForm { get; set; } = null!;
        private LabScheduleCommands.Create.Command SingleLabScheduleCommand { get; set; } = null!;

        private MudForm RangeLabScheduleForm { get; set; } = null!;
        private LabScheduleCommands.CreateRange.Command RangeLabScheduleCommand { get; set; } = null!;

        private LabQueries.Get.Response ResourceResponse { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            ResourceResponse = await Mediator.Send(new LabQueries.Get.Query(labId: LabId));
            if (ResourceResponse.Resource is not null)
            {
                SingleLabScheduleCommand = new LabScheduleCommands.Create.Command()
                {
                    LabId = ResourceResponse.Resource.Id,
                    Start = ResourceResponse.Resource.StartTime.ToTimeSpan(),
                    End = ResourceResponse.Resource.EndTime.ToTimeSpan(),
                };
                RangeLabScheduleCommand = new LabScheduleCommands.CreateRange.Command()
                {
                    LabId = ResourceResponse.Resource.Id,
                    Start = ResourceResponse.Resource.StartTime.ToTimeSpan(),
                    End = ResourceResponse.Resource.EndTime.ToTimeSpan(),
                };
            }
            else
            {
                NavigationManager.NavigateTo(uri: "/404");
            }
        }

        private Func<object, string, Task<IEnumerable<string>>> SingleLabScheduleValidateValue => async (model, propertyName) =>
        {
            var validator = new LabScheduleCommands.Create.CommandValidator(dateTimeService: DateTimeService);

            var result = await validator.ValidateAsync(ValidationContext<LabScheduleCommands.Create.Command>.CreateWithOptions((LabScheduleCommands.Create.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
        private Func<object, string, Task<IEnumerable<string>>> RangeLabScheduleValidateValue => async (model, propertyName) =>
        {
            var validator = new LabScheduleCommands.CreateRange.CommandValidator(dateTimeService: DateTimeService);

            var result = await validator.ValidateAsync(ValidationContext<LabScheduleCommands.CreateRange.Command>.CreateWithOptions((LabScheduleCommands.CreateRange.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        private async Task CreateSingleLabScheduleAsync()
        {
            await SingleLabScheduleForm.Validate();

            if (SingleLabScheduleForm.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(SingleLabScheduleCommand);

                    NavigationManager.NavigateTo(uri: $"/Labs/{result.Resource.LabId}");
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
        private async Task CreateRangeLabScheduleAsync()
        {
            await RangeLabScheduleForm.Validate();

            if (RangeLabScheduleForm.IsValid)
            {
                try
                {
                    _ = await Mediator.Send(RangeLabScheduleCommand);

                    NavigationManager.NavigateTo(uri: $"/Labs/{ResourceResponse.Resource!.Id}");
                }
                catch
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
    }
}
