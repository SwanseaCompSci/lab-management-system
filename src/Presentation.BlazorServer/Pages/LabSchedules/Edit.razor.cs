using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces.Infrastructure.Shared.Services;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabScheduleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Exceptions;
using LabQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabQueries;
using LabScheduleCommands = SwanseaCompSci.LabManagementSystem.Core.Application.Commands.LabScheduleCommands;
using LabScheduleQueries = SwanseaCompSci.LabManagementSystem.Core.Application.Queries.LabScheduleQueries;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.LabSchedules
{
    public partial class Edit
    {
        [Parameter] public Guid LabScheduleId { get; set; }

        [Inject] public IDateTimeService DateTimeService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Edit a lab schedule";

        private MudForm Form { get; set; } = null!;
        private LabScheduleCommands.Update.Command Command { get; set; } = null!;

        private LabScheduleModel? LabScheduleModel { get; set; }
        private LabModel? LabModel { get; set; } = null!;

        private Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var validator = new LabScheduleCommands.Update.CommandValidator(dateTimeService: DateTimeService);

            var result = await validator.ValidateAsync(ValidationContext<LabScheduleCommands.Update.Command>.CreateWithOptions((LabScheduleCommands.Update.Command)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            LabScheduleModel = (await Mediator.Send(new LabScheduleQueries.Get.Query(labScheduleId: LabScheduleId))).Resource;
            if (LabScheduleModel is not null)
            {
                LabModel = (await Mediator.Send(new LabQueries.Get.Query(labId: LabScheduleModel.LabId))).Resource;

                Command = new LabScheduleCommands.Update.Command()
                {
                    Id = LabScheduleModel.Id,
                    LabId = LabScheduleModel.LabId,
                    Date = new DateTime(year: LabScheduleModel.Start.Year,
                                        month: LabScheduleModel.Start.Month,
                                        day: LabScheduleModel.Start.Day),
                    Start = new TimeSpan(hours: LabScheduleModel.Start.Hour,
                                         minutes: LabScheduleModel.Start.Minute,
                                         seconds: LabScheduleModel.Start.Second),
                    End = new TimeSpan(hours: LabScheduleModel.End.Hour,
                                       minutes: LabScheduleModel.End.Minute,
                                       seconds: LabScheduleModel.End.Second),
                };
            }
            else
            {
                NavigationManager.NavigateTo(uri: $"/404");
            }
        }

        private async Task UpdateLabSchedule()
        {
            await Form.Validate();

            if (Form.IsValid)
            {
                try
                {
                    var result = await Mediator.Send(Command);
                    NavigationManager.NavigateTo(uri: $"/Labs/{result.Resource.LabId}");
                }
                catch (EntityNotFoundException)
                {
                    NavigationManager.NavigateTo(uri: "/404");
                }
                catch (Exception)
                {
                    NavigationManager.NavigateTo(uri: "/500");
                }
            }
        }
    }
}
