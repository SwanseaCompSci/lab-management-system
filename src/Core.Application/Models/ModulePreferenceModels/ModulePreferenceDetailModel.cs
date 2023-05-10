using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModuleModels;
using SwanseaCompSci.LabManagementSystem.Core.Application.Models.UserModels;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels
{
    /// <summary>
    /// A detailed <see cref="ModulePreference"/> application model.
    /// </summary>
    public class ModulePreferenceDetailModel
    {
        /// <summary>
        /// A <see cref="Domain.Entities.User"/> associated with the <see cref="ModulePreference"/>.
        /// </summary>
        public UserModel User { get; set; } = null!;

        /// <summary>
        /// A <see cref="Domain.Entities.Module"/> associated with the <see cref="ModulePreference"/>.
        /// </summary>
        public ModuleModel Module { get; set; } = null!;

        /// <summary>
        /// A <see cref="Domain.Enums.Status"/> of the <see cref="ModulePreference"/>.
        /// </summary>
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="ModulePreferenceDetailModel"/>.
    /// </summary>
    public class ModulePreferenceDetailModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ModulePreferenceDetailModelMappingProfile"/> class.
        /// </summary>
        public ModulePreferenceDetailModelMappingProfile()
        {
            CreateMap<ModulePreference, ModulePreferenceDetailModel>()
                .ForMember(x => x.Status, m => m.MapFrom(s => s.Status.ToString()));
        }
    }
}
