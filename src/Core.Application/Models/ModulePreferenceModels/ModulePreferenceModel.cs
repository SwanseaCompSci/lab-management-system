using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Models.ModulePreferenceModels
{
    /// <summary>
    /// A <see cref="ModulePreference"/> application model.
    /// </summary>
    public class ModulePreferenceModel
    {
        /// <summary>
        /// An identifier of the associated <see cref="User"/>.
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// An identifier of the associated <see cref="Module"/>.
        /// </summary>
        public Guid ModuleId { get; set; }
        /// <summary>
        /// A <see cref="Domain.Enums.Status"/> of the <see cref="ModulePreference"/>.
        /// </summary>
        public string Status { get; set; } = null!;
    }

    /// <summary>
    /// A mapping profile for <see cref="ModulePreferenceModel"/>.
    /// </summary>
    public sealed class ModulePreferenceModelMappingProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ModulePreferenceModelMappingProfile"/> class.
        /// </summary>
        public ModulePreferenceModelMappingProfile()
        {
            CreateMap<ModulePreference, ModulePreferenceModel>()
                .ForMember(x => x.Status, m => m.MapFrom(s => s.Status.ToString()));
        }
    }
}
