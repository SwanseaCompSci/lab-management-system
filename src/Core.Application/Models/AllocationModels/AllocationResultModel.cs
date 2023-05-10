using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.AllocationModels
{
    // TODO: Add docs comments
    public sealed class AllocationResultModel
    {
        public LabModel Lab { get; set; } = null!;

        public ModuleModel Module { get; set; } = null!;

        public IEnumerable<UserModel> AllocatedUsers { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="AllocationResultModel"/>.
    /// </summary>
    public sealed class AllocationResultModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="AllocationResultModelMappingProfile"/> class.
        /// </summary>
        public AllocationResultModelMappingProfile()
        {
            CreateMap<Lab, AllocationResultModel>()
                .ForMember(x => x.Lab, m => m.MapFrom(s => s))
                .ForMember(x => x.Module, m => m.MapFrom(s => s.Module))
                .ForMember(x => x.AllocatedUsers, m => m.MapFrom(s => s.UserLabs.Select(x => x.User)));
        }
    }
}
