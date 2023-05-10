using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels
{
    /// <summary>
    /// A <see cref="ModulePreference"/> model for <see cref="UserDetailModel"/>.
    /// </summary>
    public sealed class UserDetailModulePreferenceModel
    {
        /// <summary>
        /// A <see cref="Domain.Entities.Module"/> the <see cref="User"/> prefers to work in.
        /// </summary>
        public ModuleModel Module { get; set; } = null!;
        /// <summary>
        /// A module preference request status.
        /// </summary>
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="UserDetailModulePreferenceModel"/>.
    /// </summary>
    public sealed class UserDetailModulePreferenceModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserDetailModulePreferenceModelMappingProfile"/> class.
        /// </summary>
        public UserDetailModulePreferenceModelMappingProfile()
        {
            CreateMap<ModulePreference, UserDetailModulePreferenceModel>()
                .ForMember(x => x.Status, m => m.MapFrom(s => s.Status.ToString()));
        }
    }
}
