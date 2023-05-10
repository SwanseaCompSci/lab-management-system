using MediatR;
using Microsoft.AspNetCore.Components;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Queries.AllocationQueries;

namespace SwanseaCompSci.LabManagementSystem.Presentation.BlazorServer.Pages.Tools.Allocation
{
    public partial class Result
    {
        [Inject] public IMediator Mediator { get; set; } = null!;
        [Inject] public NavigationManager NavigationManager { get; set; } = null!;

        private string Title { get; } = "Allocation Result";

        private IEnumerable<AllocationResultPageModel> Allocations { get; set; } = null!;
        private string AllocationsSearchString { get; set; } = string.Empty;


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var pageModels = new List<AllocationResultPageModel>();
            var resource = (await Mediator.Send(new GetLabAllocations.Query())).Resource;

            foreach (var item in resource)
            {
                pageModels.AddRange(item.AllocatedUsers.Select(x => new AllocationResultPageModel()
                {
                    Module = item.Module,
                    Lab = item.Lab,
                    User = x,
                }));
            }

            Allocations = pageModels;
        }

        private bool FilterAllocationsFunction(AllocationResultPageModel allocation) => FilterAllocationsFunction(allocation: allocation, searchString: AllocationsSearchString);
        private bool FilterAllocationsFunction(AllocationResultPageModel allocation, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            else if (allocation.Module.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (allocation.Module.Code.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (allocation.Module.Level.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            else if (allocation.Lab.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (allocation.Lab.Day.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            else if (allocation.User.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (allocation.User.Surname.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (allocation.User.AchievedLevel.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private class AllocationResultPageModel
        {
            public ModuleModel Module { get; set; } = null!;
            public LabModel Lab { get; set; } = null!;
            public UserModel User { get; set; } = null!;
        }
    }
}
