using AutoMapper;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Entities;
using SwanseaCompSci.LabManagementSystem.Core.Domain.Enums;

namespace SwanseaCompSci.LabManagementSystem.Core.Application.Allocation.Models
{
    // TODO: Add docs comments
    public sealed class ModulePreferenceModel
    {
        public Guid UserId { get; set; }
        public Guid ModuleId { get; set; }
        public Status Status { get; set; }
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
            CreateMap<ModulePreference, ModulePreferenceModel>();
        }
    }
}
