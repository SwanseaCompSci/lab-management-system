using MediatR;
using Microsoft.AspNetCore.Components;
using SwanseaCompSci.LabManagementSystem.Core.Application.Common.Interfaces;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.DashboardModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.DashboardQueries;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.UserQueries;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages
{
    public partial class Index
    {
        [Inject] public ICurrentUserService CurrentUserService { get; set; } = null!;
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Dashboard";

        private UserModel? User { get; set; } = null!;
        private IEnumerable<WorkShiftModel> WorkShifts { get; set; } = null!;

        private bool QuestionnaireAlertClosed { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            User = (await Mediator.Send(new Get.Query(userId: CurrentUserService.UserId!.Value))).Resource;

            WorkShifts = (await Mediator.Send(new GetFutureWorkShifts.Query(userId: CurrentUserService.UserId.Value)))
                .Resource
                .OrderBy(x => x.LabScheduleStart);
        }
    }
}
