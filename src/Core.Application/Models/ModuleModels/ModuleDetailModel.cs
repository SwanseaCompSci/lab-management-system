using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.LabModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels
{
    // TODO: Add docs comments

    /// <summary>
    /// A detailed <see cref="Module"/> application model.
    /// </summary>
    public class ModuleDetailModel : ModuleModel
    {
        public IEnumerable<ModulePreferenceDetailModel> ModulePreferences { get; set; } = null!;
        public IEnumerable<LabModel> Labs { get; set; } = null!;
        public IEnumerable<ModuleDetailUserRoleModel> UserRoles { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="ModuleDetailModel"/>.
    /// </summary>
    public sealed class ModuleDetailModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ModuleDetailModelMappingProfile"/> class.
        /// </summary>
        public ModuleDetailModelMappingProfile()
        {
            CreateMap<Module, ModuleDetailModel>()
                .IncludeBase<Module, ModuleModel>()
                .ForMember(x => x.UserRoles, m => m.MapFrom(x => x.UserModules));
        }
    }
}
